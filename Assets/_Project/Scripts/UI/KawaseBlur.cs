using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UntitledBallGame.UI
{
    public class KawaseBlur : ScriptableRendererFeature
    {
        [System.Serializable]
        public class KawaseBlurSettings
        {
            public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
            public Material blurMaterial;

            [Range(2, 15)] public int blurPasses = 1;

            [Range(1, 4)] public int downsample = 1;
            public bool copyToFramebuffer;
            public string targetName = "_blurTexture";
        }

        public KawaseBlurSettings settings = new KawaseBlurSettings();

        class CustomRenderPass : ScriptableRenderPass
        {
            public Material BlurMaterial;
            public int Passes;
            public int Downsample;
            public bool CopyToFramebuffer;
            public string TargetName;
            readonly string _profilerTag;

            int _tmpId1;
            int _tmpId2;

            RenderTargetIdentifier _tmpRt1;
            RenderTargetIdentifier _tmpRt2;

            private RenderTargetIdentifier Source { get; set; }

            public void Setup(RenderTargetIdentifier source)
            {
                Source = source;
            }

            public CustomRenderPass(string profilerTag)
            {
                _profilerTag = profilerTag;
            }

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                var width = cameraTextureDescriptor.width / Downsample;
                var height = cameraTextureDescriptor.height / Downsample;

                _tmpId1 = Shader.PropertyToID("tmpBlurRT1");
                _tmpId2 = Shader.PropertyToID("tmpBlurRT2");
                cmd.GetTemporaryRT(_tmpId1, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
                cmd.GetTemporaryRT(_tmpId2, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);

                _tmpRt1 = new RenderTargetIdentifier(_tmpId1);
                _tmpRt2 = new RenderTargetIdentifier(_tmpId2);

                ConfigureTarget(_tmpRt1);
                ConfigureTarget(_tmpRt2);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                CommandBuffer cmd = CommandBufferPool.Get(_profilerTag);

                RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
                opaqueDesc.depthBufferBits = 0;

                // first pass
                // cmd.GetTemporaryRT(tmpId1, opaqueDesc, FilterMode.Bilinear);
                cmd.SetGlobalFloat("_offset", 1.5f);
                cmd.Blit(Source, _tmpRt1, BlurMaterial);

                for (var i = 1; i < Passes - 1; i++)
                {
                    cmd.SetGlobalFloat("_offset", 0.5f + i);
                    cmd.Blit(_tmpRt1, _tmpRt2, BlurMaterial);

                    // pingpong
                    var rttmp = _tmpRt1;
                    _tmpRt1 = _tmpRt2;
                    _tmpRt2 = rttmp;
                }

                // final pass
                cmd.SetGlobalFloat("_offset", 0.5f + Passes - 1f);
                if (CopyToFramebuffer)
                {
                    cmd.Blit(_tmpRt1, Source, BlurMaterial);
                }
                else
                {
                    cmd.Blit(_tmpRt1, _tmpRt2, BlurMaterial);
                    cmd.SetGlobalTexture(TargetName, _tmpRt2);
                }

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                CommandBufferPool.Release(cmd);
            }

            public override void FrameCleanup(CommandBuffer cmd)
            {
            }
        }

        CustomRenderPass _scriptablePass;

        public override void Create()
        {
            _scriptablePass = new CustomRenderPass("KawaseBlur");
            _scriptablePass.BlurMaterial = settings.blurMaterial;
            _scriptablePass.Passes = settings.blurPasses;
            _scriptablePass.Downsample = settings.downsample;
            _scriptablePass.CopyToFramebuffer = settings.copyToFramebuffer;
            _scriptablePass.TargetName = settings.targetName;

            _scriptablePass.renderPassEvent = settings.renderPassEvent;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            var src = renderer.cameraColorTarget;
            _scriptablePass.Setup(src);
            renderer.EnqueuePass(_scriptablePass);
        }
    }
}