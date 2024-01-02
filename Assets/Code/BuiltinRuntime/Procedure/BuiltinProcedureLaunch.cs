using GameFramework.Localization;
using System;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// 内置运行时流程入口
    /// </summary>
    internal class BuiltinProcedureLaunch:BuiltinProcedureBase
    {
        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Debug("Launch the framework and start loading the application's default configuration.");
            //初始化构建配置
            GameCollectionEntry.BuiltinData.InitBuildInfo( );
            //初始化字典配置
            GameCollectionEntry.BuiltinData.InitDefaultDictionary( );
            //初始化默认语言配置
            InitLanguageSettings( );
            //初始化变体配置
            InitCurrentVariant( );
            //初始化声音配置
            InitSoundSettings( );
            //初始化加载界面配置
            GameCollectionEntry.BuiltinData.InitResourceUI( );
            //到这里应用基础配置加载完毕:300毫秒
            IsEnterNextProduce = true;
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner , float elapseSeconds , float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner , elapseSeconds , realElapseSeconds);

            if(!IsEnterNextProduce)
            {
                return;
            }
            ChangeState(procedureOwner , typeof(BuiltinProcedureAuthorization));
        }

        /// <summary>
        /// 初始化语言配置
        /// </summary>
        private void InitLanguageSettings( )
        {
            if(GameCollectionEntry.Base.EditorResourceMode && GameCollectionEntry.Base.EditorLanguage != Language.Unspecified)
            {
                return;
            }
            Language language = GameCollectionEntry.Base.EditorLanguage;
            string languageString = GameCollectionEntry.Setting.GetString(AppBuiltinConfig.Setting.Language);
            if(!languageString.IsNullOrEmpty( ))
            {
                try
                {
                    language = (Language)Enum.Parse(typeof(Language) , languageString);
                }
                catch
                {

                }
            }
            if(!LanguageSupport(language))
            {
                //若是暂不支持得语言,则使用英语
                language = Language.English;
            }
            GameCollectionEntry.Setting.SetString(AppBuiltinConfig.Setting.Language , language.ToString( ));
            GameCollectionEntry.Setting.Save( );
        }


        /// <summary>
        /// 初始化变体配置
        /// </summary>
        private void InitCurrentVariant( )
        {
            if(GameCollectionEntry.Base.EditorResourceMode)
            {
                return;
            }

            string currentVariant = GameCollectionEntry.Localization.Language switch
            {
                Language.English => "en-us",
                Language.ChineseSimplified => "zh-cn",
                Language.ChineseTraditional => "zh-tw",
                Language.Korean => "ko-kr",
                _ => "zh-cn",
            };
            GameCollectionEntry.Resource.SetCurrentVariant(currentVariant);
            Log.Debug("Init current variant complete");
        }

        /// <summary>
        /// 初始化声音配置
        /// </summary>
        private void InitSoundSettings( )
        {

            Log.Debug("Init sound settings complete.");
        }

        /// <summary>
        /// 是否支持该语言
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        private bool LanguageSupport(Language language)
        {
            switch(language)
            {
                case Language.English:
                case Language.ChineseSimplified:
                case Language.ChineseTraditional:
                case Language.Korean:
                    return true;
                default:
                    break;
            }
            return false;
        }
    }
}
