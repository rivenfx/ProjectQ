using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Riven;
using Riven.Localization;
using Riven.Modular;

namespace Company.Project
{
    [DependsOn(

        )]
    public class CompanyProjectDomainModule : AppModule
    {
        public override void OnPreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.RegisterAssemblyOf<CompanyProjectDomainModule>();
        }

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {

        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {


            #region Riven - 初始化本地化

            this.InitLocalization(context);

            #endregion

        }

        #region 内部辅助函数

        /// <summary>
        /// 初始化本地化
        /// </summary>
        /// <param name="context"></param>
        protected void InitLocalization(ApplicationInitializationContext context)
        {
            var assemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            var sourceFilePath = Path.Combine(assemblyPath, "Localization", "SourceFiles", "Json");
            var languageInfos = LanguageLoaderWithFile.FromFolderWithJson(sourceFilePath);

            context.ServiceProvider.AddOrUpdateLanguages(languageInfos);

            var languageManager = context.ServiceProvider.GetService<ILanguageManager>();
            if (languageManager.GetEnabledLanguages().Count == 0)
            {
                throw new ArgumentException($"启用语言数量为0");
            }

            languageManager.ChangeDefaultLanguage(AppConsts.DefaultLanguage);
        } 

        #endregion
    }
}
