using GameFramework.Localization;
using System;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// 流程入口
    /// </summary>
    internal class BuiltinProcedureProcedureLaunch:BuiltinProcedureBase
    {
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Info($"<color=lime>进入<主入口>流程.</color>");
            WTGame.AppBuiltinConfigs.InitLoadLanguageConfigData( );
            // 语言配置：设置当前使用的语言，如果不设置，则默认使用操作系统语言。
            InitLanguageSettings( );
            // 变体配置：根据使用的语言，通知底层加载对应的资源变体。
            InitCurrentVariant( );
            // 声音配置：根据用户配置数据，设置即将使用的声音选项。
            InitSoundSettings( );

            WTGame.BuiltinData.InitDefalutResourceUI( );
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);
            //运行一帧即切换到验证流程
            ChangeState<BuiltinProcedureAuthorization>(procedureOwner);
        }

        /// <summary>
        /// 初始化语言设置
        /// </summary>
        private void InitLanguageSettings( )
        {
            if(WTGame.Base.EditorResourceMode && WTGame.Base.EditorLanguage != Language.Unspecified)
            {
                return;
            }
            Language language = WTGame.Localization.Language;
            string languageString = WTGame.Setting.GetString(BuiltinRuntimeUtility.Settings.Language);
            if(!string.IsNullOrEmpty(languageString))
            {
                try
                {
                    language = (Language)Enum.Parse(typeof(Language) , languageString);
                }
                catch
                {
                }
            }
            else
            {
                language = Language.ChineseSimplified;
                WTGame.Setting.SetString(BuiltinRuntimeUtility.Settings.Language , language.ToString( ));
                WTGame.Setting.Save( );
            }

            if(language != Language.English && language != Language.ChineseSimplified && language != Language.ChineseTraditional && language != Language.Korean)
            {
                // 若是暂不支持的语言，则使用中文
                language = Language.ChineseSimplified;
                WTGame.Setting.SetString(BuiltinRuntimeUtility.Settings.Language , language.ToString( ));
                WTGame.Setting.Save( );
            }
            WTGame.Localization.Language = language;
            Log.Info("初始化语言配置完成,当前语言为{0}" , language.ToString( ));
        }

        /// <summary>
        /// 初始化变体
        /// </summary>
        private void InitCurrentVariant( )
        {
            if(WTGame.Base.EditorResourceMode)
            {
                // 编辑器资源模式不使用 AssetBundle，也就没有变体了
                return;
            }

            string currentVariant = null;
            switch(WTGame.Localization.Language)
            {
                case Language.English:
                    currentVariant = "en-us";
                    break;

                case Language.ChineseSimplified:
                    currentVariant = "zh-cn";
                    break;

                case Language.ChineseTraditional:
                    currentVariant = "zh-tw";
                    break;

                case Language.Korean:
                    currentVariant = "ko-kr";
                    break;

                default:
                    currentVariant = "zh-cn";
                    break;
            }

            WTGame.Resource.SetCurrentVariant(currentVariant);
        }

        /// <summary>
        /// 初始化声音配置
        /// </summary>
        private void InitSoundSettings( )
        {

        }
    }
}
