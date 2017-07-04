using UnityEngine;
using UnityEngine.Rendering;

public class Tester : MonoBehaviour
{
    [SerializeField, HideInInspector] ComputeShader _writerShader;
    [SerializeField, HideInInspector] Shader _readerShader;

    Material _readerMaterial;
    RenderTexture _buffer;

    void Start()
    {
        _readerMaterial = new Material(_readerShader);

        _buffer = new RenderTexture(8, 8, 0, RenderTextureFormat.RHalf);
        _buffer.enableRandomWrite = true;
        _buffer.dimension = TextureDimension.Tex2DArray;
        _buffer.volumeDepth = 8;
        _buffer.Create();
    }

    void OnDestroy()
    {
        if (_readerMaterial != null) Destroy(_readerMaterial);
        if (_buffer != null) Destroy(_buffer);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var kernel = _writerShader.FindKernel("WriterMain");
        _writerShader.SetTexture(kernel, "BufferTex", _buffer);
        _writerShader.Dispatch(kernel, 1, 1, 1);

        _readerMaterial.SetTexture("_BufferTex", _buffer);
        Graphics.Blit(null, destination, _readerMaterial, 0);
    }
}
