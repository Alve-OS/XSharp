﻿using System;
using System.Composition;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Build;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;
using Task = System.Threading.Tasks.Task;

namespace XSharp.ProjectSystem.VS.Build
{
    [Export(typeof(IPublishProvider))]
    [AppliesTo(ProjectCapability.XSharp)]
    internal class XSharpPublishProvider : IPublishProvider
    {
        [Import]
        private ProjectProperties ProjectProperties { get; set; }

        [Import]
        private SVsServiceProvider ServiceProvider { get; set; }

        [Import]
        private IProjectThreadingService ProjectThreadingService { get; set; }

        private PublishSettings mPublishSettings;

        public async Task<bool> IsPublishSupportedAsync()
        {
            // todo: when does this method get called? on load or on project changed too?
            var xConfiguration = await ProjectProperties.GetConfigurationGeneralPropertiesAsync();
            return await xConfiguration.OutputType.GetEvaluatedValueAtEndAsync() == "Bootable";
        }

        public async Task PublishAsync(CancellationToken aCancellationToken, TextWriter aOutputPaneWriter)
        {
            if (mPublishSettings == null)
            {
                await aOutputPaneWriter.WriteAsync("Publish settings are null!");
                return;
            }

            switch (mPublishSettings.PublishType)
            {
                case PublishType.USB:
                    await aOutputPaneWriter.WriteLineAsync("Publishing USB!");
                    break;
                case PublishType.PXE:
                    await aOutputPaneWriter.WriteLineAsync("Publishing PXE!");
                    break;
                default:
                    await aOutputPaneWriter.WriteLineAsync("Unknown publish type! Publish type: '{mPublishSettings.PublishType}'");
                    break;
            }

            await aOutputPaneWriter.WriteLineAsync("Publish successful!");
        }

        public Task<bool> ShowPublishPromptAsync()
        {
            ProjectThreadingService.SwitchToUIThread();

            var xPublishWindow = new PublishWindow();
            mPublishSettings = xPublishWindow.ShowModal();

            return Task.FromResult(mPublishSettings != null);
        }
    }
}