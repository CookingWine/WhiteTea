using GameFramework.DataTable;
using GameFramework.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using WhiteTea.BuiltinRuntime;
namespace WhiteTea.HotfixLogic
{
    /// <summary>
    /// ui扩展
    /// </summary>
    public static class UIExtension
    {
        public static IEnumerator FadeToAlpha(this CanvasGroup canvasGroup , float alpha , float duration)
        {
            float time = 0f;
            float originalAlpha = canvasGroup.alpha;
            while(time < duration)
            {
                time += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(originalAlpha , alpha , time / duration);
                yield return new WaitForEndOfFrame( );
            }

            canvasGroup.alpha = alpha;
        }
        public static IEnumerator SmoothValue(this Slider slider , float value , float duration)
        {
            float time = 0f;
            float originalValue = slider.value;
            while(time < duration)
            {
                time += Time.deltaTime;
                slider.value = Mathf.Lerp(originalValue , value , time / duration);
                yield return new WaitForEndOfFrame( );
            }

            slider.value = value;
        }
        public static int? OpenUIForm(this UIComponent uiComponent , UIFormId uiFormId , object userData = null)
        {
            return uiComponent.OpenUIForm((int)uiFormId , userData);
        }
        public static int? OpenUIForm(this UIComponent uiComponent , int uiFormId , object userData = null)
        {
            IDataTable<DRUIForm> dtUIForm = WTGame.DataTable.GetDataTable<DRUIForm>( );
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
            if(drUIForm == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table." , uiFormId.ToString( ));
                return null;
            }

            string assetName = BuiltinRuntimeUtility.AssetsUtility.GetUIFormAsset(drUIForm.AssetPath);
            if(!drUIForm.AllowMultiInstance)
            {
                if(uiComponent.IsLoadingUIForm(assetName))
                {
                    return null;
                }

                if(uiComponent.HasUIForm(assetName))
                {
                    return null;
                }
            }

            return uiComponent.OpenUIForm(assetName , drUIForm.UIGroupName , 50 , drUIForm.PauseCoveredUIForm , userData);
        }
        public static bool HasUIForm(this UIComponent uiComponent , int uiFormId , string uiGroupName = null)
        {
            IDataTable<DRUIForm> dtUIForm = WTGame.DataTable.GetDataTable<DRUIForm>( );
            DRUIForm drUIForm = dtUIForm.GetDataRow(uiFormId);
            if(drUIForm == null)
            {
                return false;
            }

            string assetName = BuiltinRuntimeUtility.AssetsUtility.GetUIFormAsset(drUIForm.AssetPath);
            if(string.IsNullOrEmpty(uiGroupName))
            {
                return uiComponent.HasUIForm(assetName);
            }

            IUIGroup uiGroup = uiComponent.GetUIGroup(uiGroupName);
            if(uiGroup == null)
            {
                return false;
            }

            return uiGroup.HasUIForm(assetName);
        }
    }
}
