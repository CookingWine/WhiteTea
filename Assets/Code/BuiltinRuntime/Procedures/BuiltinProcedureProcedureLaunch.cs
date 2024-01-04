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
    }
}
