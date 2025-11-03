using UnityEngine;
using System.Collections;

public class ParallaxManager : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform[] instances;
        public Transform[] transitionInstances;
        public float speedMultiplier = 1f;
        [HideInInspector] public float width;
    }

    [System.Serializable]
    public class Theme
    {
        public string name;
        public Sprite far, mid, near;
    }

    [Header("Camadas")]
    public ParallaxLayer far, mid, near;

    [Header("Temas")]
    public Theme day, night;

    [Header("Configuração")]
    public float baseSpeed = 1f;
    public bool startNight = false;
    [Tooltip("Quantos pontos para trocar entre dia/noite")]
    public int pointsPerCycle = 2;

    [Header("Transição")]
    public float transitionDuration = 0.8f;

    private Camera cam;
    private Theme current;
    private float leftEdge;
    private bool isNight = false;
    private int lastCycle = 0;
    private Coroutine[] transitionCoroutines = new Coroutine[3];

    void Start()
    {
        cam = Camera.main;
        current = startNight ? night : day;
        isNight = startNight;

        float camWidth = cam.orthographicSize * cam.aspect;
        leftEdge = cam.transform.position.x - camWidth;

        InitLayer(far);
        InitLayer(mid);
        InitLayer(near);

        ApplyTheme(current);

        ResetLayerAlpha(far);
        ResetLayerAlpha(mid);
        ResetLayerAlpha(near);
    }

    void InitLayer(ParallaxLayer layer)
    {
        if (layer == null || layer.instances == null || layer.instances.Length < 2) return;

        var sr = layer.instances[0].GetComponent<SpriteRenderer>();
        if (!sr || !sr.sprite) return;

        layer.width = sr.bounds.size.x;
        float y = layer.instances[0].position.y;

        for (int i = 0; i < layer.instances.Length; i++)
        {
            float x = layer.instances[0].position.x + i * layer.width;
            layer.instances[i].position = new Vector3(x, y, layer.instances[i].position.z);

            // Garante Tiled Mode no original
            sr = layer.instances[i].GetComponent<SpriteRenderer>();
            if (sr)
            {
                sr.drawMode = SpriteDrawMode.Tiled;
                sr.tileMode = SpriteTileMode.Continuous;
            }
        }
    }

    void Update()
    {
        float speed = baseSpeed * Time.deltaTime;
        Move(far, speed * 0.3f * far.speedMultiplier);
        Move(mid, speed * 0.6f * mid.speedMultiplier);
        Move(near, speed * 1.0f * near.speedMultiplier);

        float camWidth = cam.orthographicSize * cam.aspect;
        leftEdge = cam.transform.position.x - camWidth;
    }

    void Move(ParallaxLayer layer, float speed)
    {
        if (layer == null || layer.instances == null || layer.instances.Length == 0) return;

        foreach (Transform t in layer.instances)
            t.position += Vector3.left * speed;

        if (layer.transitionInstances != null)
            foreach (Transform t in layer.transitionInstances)
                t.position += Vector3.left * speed;

        for (int i = 0; i < layer.instances.Length; i++)
        {
            Transform t = layer.instances[i];
            if (t.position.x + (layer.width / 2f) < leftEdge)
            {
                Transform rightmost = GetRightmost(layer.instances);
                t.position = new Vector3(rightmost.position.x + layer.width, t.position.y, t.position.z);

                if (layer.transitionInstances != null && i < layer.transitionInstances.Length)
                    layer.transitionInstances[i].position = t.position;
            }
        }
    }

    private Transform GetRightmost(Transform[] instances)
    {
        Transform rightmost = instances[0];
        foreach (var inst in instances)
            if (inst.position.x > rightmost.position.x)
                rightmost = inst;
        return rightmost;
    }

    public void OnScoreChanged(int currentScore)
    {
        int currentCycle = currentScore / pointsPerCycle;
        if (currentCycle != lastCycle)
        {
            ToggleDayNight();
            lastCycle = currentCycle;
        }
    }

    private void ToggleDayNight()
    {
        isNight = !isNight;
        SetNightMode(isNight);
    }

    public void SetNightMode(bool nightMode)
    {
        isNight = nightMode;
        current = isNight ? night : day;
        ApplyTheme(current);
    }

    void ApplyTheme(Theme newTheme)
    {
        if (newTheme == null) return;
        StartLayerTransition(far, newTheme.far, 0);
        StartLayerTransition(mid, newTheme.mid, 1);
        StartLayerTransition(near, newTheme.near, 2);
    }

    private void StartLayerTransition(ParallaxLayer layer, Sprite newSprite, int index)
    {
        if (layer.instances == null || layer.instances.Length == 0 || newSprite == null) return;

        if (transitionCoroutines[index] != null)
            StopCoroutine(transitionCoroutines[index]);

        CreateTransitionInstances(layer);
        SetSprite(layer.transitionInstances, newSprite);

        var newRenderer = layer.transitionInstances[0].GetComponent<SpriteRenderer>();
        float newWidth = newRenderer.bounds.size.x;

        if (Mathf.Abs(layer.width - newWidth) > 0.01f)
        {
            layer.width = newWidth;
            RefreshLayerAfterSpriteChange(layer);
        }

        AlignTransitionInstances(layer);
        transitionCoroutines[index] = StartCoroutine(FadeTransition(layer, index));
    }

    // ==================== MÉTODO CORRIGIDO: TILED + CONTINUOUS ====================
    private void CreateTransitionInstances(ParallaxLayer layer)
    {
        // Reutiliza se já existir
        if (layer.transitionInstances != null && layer.transitionInstances.Length == layer.instances.Length)
        {
            foreach (var t in layer.transitionInstances)
            {
                var sr = t.GetComponent<SpriteRenderer>();
                if (sr)
                {
                    sr.color = new Color(1, 1, 1, 0);
                    sr.drawMode = SpriteDrawMode.Tiled;
                    sr.tileMode = SpriteTileMode.Continuous;
                }
            }
            return;
        }

        // Destroi antigas
        if (layer.transitionInstances != null)
        {
            foreach (var t in layer.transitionInstances)
                if (t != null) Destroy(t.gameObject);
        }

        var list = new System.Collections.Generic.List<Transform>();
        for (int i = 0; i < layer.instances.Length; i++)
        {
            Transform original = layer.instances[i];
            var originalSR = original.GetComponent<SpriteRenderer>();

            GameObject go = new GameObject($"Transition_{original.name}");
            go.transform.parent = original.parent;
            go.transform.position = original.position;
            go.transform.localScale = original.localScale;

            var sr = go.AddComponent<SpriteRenderer>();

            // COPIA TUDO DO ORIGINAL
            sr.sprite = originalSR.sprite;
            sr.drawMode = originalSR.drawMode;           // Tiled
            sr.size = originalSR.size;                   // (width, 2.5)
            sr.tileMode = originalSR.tileMode;           // Continuous
            sr.sortingLayerName = originalSR.sortingLayerName;
            sr.sortingOrder = originalSR.sortingOrder;
            sr.color = new Color(1, 1, 1, 0);
            sr.flipX = originalSR.flipX;
            sr.flipY = originalSR.flipY;

            list.Add(go.transform);
        }
        layer.transitionInstances = list.ToArray();
    }

    private void AlignTransitionInstances(ParallaxLayer layer)
    {
        if (layer.transitionInstances == null) return;
        for (int i = 0; i < layer.instances.Length; i++)
        {
            if (i < layer.transitionInstances.Length)
                layer.transitionInstances[i].position = layer.instances[i].position;
        }
    }

    private IEnumerator FadeTransition(ParallaxLayer layer, int index)
    {
        float timer = 0f;
        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / transitionDuration);

            foreach (var t in layer.transitionInstances)
            {
                var sr = t.GetComponent<SpriteRenderer>();
                if (sr)
                {
                    Color c = sr.color;
                    c.a = alpha;
                    sr.color = c;
                }
            }

            foreach (var t in layer.instances)
            {
                var sr = t.GetComponent<SpriteRenderer>();
                if (sr)
                {
                    Color c = sr.color;
                    c.a = 1f - alpha;
                    sr.color = c;
                }
            }

            yield return null;
        }

        SwapLayers(layer);
        transitionCoroutines[index] = null;
    }

    private void SwapLayers(ParallaxLayer layer)
    {
        var temp = layer.instances;
        layer.instances = layer.transitionInstances;
        layer.transitionInstances = temp;

        foreach (var t in layer.transitionInstances)
            if (t != null) Destroy(t.gameObject);

        layer.transitionInstances = null;

        foreach (var t in layer.instances)
        {
            var sr = t.GetComponent<SpriteRenderer>();
            if (sr)
            {
                Color c = sr.color;
                c.a = 1f;
                sr.color = c;
            }
        }
    }

    void SetSprite(Transform[] instances, Sprite s)
    {
        if (instances == null || s == null) return;
        foreach (var it in instances)
        {
            var sr = it.GetComponent<SpriteRenderer>();
            if (sr) sr.sprite = s;
        }
    }

    void ResetLayerAlpha(ParallaxLayer layer)
    {
        if (layer.instances == null) return;
        foreach (var t in layer.instances)
        {
            var sr = t.GetComponent<SpriteRenderer>();
            if (sr)
            {
                Color c = sr.color;
                c.a = 1f;
                sr.color = c;
            }
        }
    }

    public void RefreshAllLayers()
    {
        RefreshLayerAfterSpriteChange(far);
        RefreshLayerAfterSpriteChange(mid);
        RefreshLayerAfterSpriteChange(near);
    }

    void RefreshLayerAfterSpriteChange(ParallaxLayer layer)
    {
        if (layer == null || layer.instances == null || layer.instances.Length == 0) return;
        var sr0 = layer.instances[0].GetComponent<SpriteRenderer>();
        if (sr0 == null || sr0.sprite == null) return;

        layer.width = sr0.bounds.size.x;
        System.Array.Sort(layer.instances, (a, b) => a.position.x.CompareTo(b.position.x));

        float y = layer.instances[0].position.y;
        for (int i = 0; i < layer.instances.Length; i++)
        {
            layer.instances[i].position = new Vector3(
                layer.instances[0].position.x + i * layer.width,
                y,
                layer.instances[i].position.z
            );
        }

        if (layer.transitionInstances != null)
            AlignTransitionInstances(layer);
    }
}