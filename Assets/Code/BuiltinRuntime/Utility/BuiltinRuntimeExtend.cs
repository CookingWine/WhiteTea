using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.IO;

namespace WhiteTea.BuiltinRuntime
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class BuiltinRuntimeExtend
    {
        /// <summary>
		/// 查找子物体
		/// </summary>
		public static Transform FindTransform(this Transform parent , string name)
        {
            try
            {
                Transform child = parent.Find(name);
                if(child != null)
                {
                    return child;
                }
                for(int i = 0; i < parent.childCount; ++i)
                {
                    Transform transform = parent.GetChild(i);
                    if(transform.childCount > 0)
                    {
                        Transform childTransform = transform.FindTransform(name);
                        if(childTransform != null)
                        {
                            return childTransform;
                        }
                    }
                }
                return null;
            }
            catch(Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }
            return null;
        }

        /// <summary>
        /// 用名字和组件类型查找组件
        /// </summary>
        public static T FindComponent<T>(this Transform parent , string name) where T : Component
        {
            Transform trans = FindTransform(parent , name);
            if(trans == null)
            {
                return null;
            }
            return trans.GetComponent<T>( );
        }

        /// <summary>
        /// 用组件类型查找组件
        /// </summary>
        public static T FindComponent<T>(this Transform parent) where T : Component
        {
            if(parent.TryGetComponent<T>(out var c))
                return c;
            for(int i = 0; i < parent.childCount; ++i)
            {
                Transform transform = parent.GetChild(i);
                T comp = transform.GetComponent<T>( );
                if(comp != null)
                {
                    return comp;
                }
                if(transform.childCount > 0)
                {
                    T childComp = FindComponent<T>(transform);
                    if(childComp != null)
                    {
                        return childComp;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 设置物体transform信息到默认值
        /// </summary>
        /// <param name="target">目前物体</param>
        /// <param name="localPostion">true为本地位置，false为世界位置</param>
        public static void SetTransformInfoToDefalut(this Transform target , bool localPostion = true)
        {
            if(localPostion)
            {
                target.SetLocalPositionAndRotation(Vector3.zero , Quaternion.identity);
            }
            else
            {
                target.SetPositionAndRotation(Vector3.zero , Quaternion.identity);
            }
            target.localScale = Vector3.one;
        }

        /// <summary>
        /// 设置物体Transform信息
        /// </summary>
        /// <param name="target">物体</param>
        /// <param name="post">位置</param>
        /// <param name="quaternion">旋转</param>
        /// <param name="scale">缩放</param>
        /// <param name="localPostion">true为本地位置，false为世界位置</param>
        public static void SetTransformInfo(this Transform target , Vector3 post , Quaternion quaternion , Vector3 scale , bool localPostion = true)
        {
            if(localPostion)
            {
                target.SetLocalPositionAndRotation(post , quaternion);
            }
            else
            {
                target.SetPositionAndRotation(post , quaternion);
            }
            target.localScale = scale;
        }

        public static Texture2D LoadTexture(string path , int width = 1920 , int height = 1080)
        {
            if(string.IsNullOrEmpty(path))
            {
                throw new Exception("path is null");
            }
            FileStream fs = new FileStream(path , FileMode.Open , FileAccess.Read);
            fs.Seek(0 , SeekOrigin.Begin);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes , 0 , (int)fs.Length);
            fs.Close( );
            fs.Dispose( );
            Texture2D texture = new Texture2D(width , height);
            texture.LoadImage(bytes);
            return texture;
        }
        /// <summary>
        /// Texture2D转Sprite
        /// </summary>
        /// <param name="texutre"></param>
        /// <returns></returns>
        public static Sprite Texture2DToSprite(this Texture2D texutre)
        {
            return Sprite.Create(texutre , new Rect(0 , 0 , texutre.width , texutre.height) , Vector2.zero);
        }
        /// <summary>
        /// 初始化权重容器
        /// </summary>
        /// <param name="rewardWeight">权重容器</param>
        /// <param name="weightList">权重列表</param>
        public static void InitWeightContainer(out (float, int)[] rewardWeight , int[] weightList)
        {
            if(weightList == null)
            {
                throw new Exception("weight list is null");
            }
            if(weightList.Length == 0)
            {
                throw new Exception("weight list length is 0");
            }

            //计算权重的总和
            var total = weightList.Sum( );

            int length = weightList.Length;
            //取权重的平均值
            var avg = 1f * total / length;
            //最小值的平均值
            List<(float, int)> smallAvg = new List<(float, int)>( );
            //最大值的平均值
            List<(float, int)> bigAvg = new List<(float, int)>( );

            //判断每一个权重是否大于平均值，分别添加到不同的容器内
            for(int i = 0; i < weightList.Length; i++)
            {
                ( weightList[i] > avg ? bigAvg : smallAvg ).Add((weightList[i], i));
            }

            //初始化权重容器
            rewardWeight = new (float, int)[weightList.Length];

            for(int i = 0; i < weightList.Length; i++)
            {
                if(smallAvg.Count > 0)
                {
                    if(bigAvg.Count > 0)
                    {
                        rewardWeight[smallAvg[0].Item2] = (smallAvg[0].Item1 / avg, bigAvg[0].Item2);
                        bigAvg[0] = (bigAvg[0].Item1 - avg + smallAvg[0].Item1, bigAvg[0].Item2);
                        if(avg - bigAvg[0].Item1 > 0.0000001f)
                        {
                            smallAvg.Add(bigAvg[0]);
                            bigAvg.RemoveAt(0);
                        }
                    }
                    else
                    {
                        rewardWeight[smallAvg[0].Item2] = (smallAvg[0].Item1 / avg, smallAvg[0].Item2);
                    }
                    smallAvg.RemoveAt(0);
                }
                else
                {
                    rewardWeight[bigAvg[0].Item2] = (bigAvg[0].Item1 / avg, bigAvg[0].Item2);
                    bigAvg.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// 获取随机索引
        /// </summary>
        /// <param name="rewardWeight">权重容器</param>
        /// <param name="random">随机</param>
        /// <returns>获取该权重的索引</returns>
        public static int GetRandomWeightIndex((float, int)[] rewardWeight , System.Random random)
        {
            if(rewardWeight == null)
            {
                throw new Exception("reward weight is null");
            }
            if(rewardWeight.Length == 0)
            {
                throw new Exception("reward weight length = 0");
            }
            var randomNum = random.NextDouble( ) * rewardWeight.Length;
            int intRan = (int)Math.Floor(randomNum);
            var p = rewardWeight[intRan];
            if(p.Item1 > randomNum - intRan)
            {
                return intRan;
            }
            else
            {
                return p.Item2;
            }
        }

        /// <summary>
		/// 生成多边形mesh网格
		/// </summary>
		/// <param name="vector3">多边形顶点数组</param>
		/// <returns>网格</returns>
		public static Mesh GenerateMesh(this Vector3[] vector3)
        {
            Mesh mesh = new Mesh( );
            List<int> triangls = new List<int>( );
            for(int i = 0; i < vector3.Length - 1; i++)
            {
                triangls.Add(i);
                triangls.Add(i + 1);
                triangls.Add(vector3.Length - i - 1);
            }
            mesh.vertices = vector3;
            mesh.triangles = triangls.ToArray( );
            mesh.RecalculateBounds( );
            mesh.RecalculateNormals( );
            return mesh;
        }

        ///<summary>音频转字节组</summary>
		public static byte[] GetAudioByteArray(this AudioClip clip)
        {
            float[] data = new float[clip.samples];
            clip.GetData(data , 0);
            int rescaleFactor = 32767;
            byte[] outData = new byte[data.Length * 2];
            for(int i = 0; i < data.Length; i++)
            {
                //TODO:因为float数据在-1~1之间，需要把数组转换到有符号2个字节的范围-32768~32767。因此这里乘以32767
                short temshort = (short)( data[i] * rescaleFactor );
                byte[] temdata = BitConverter.GetBytes(temshort);
                outData[i * 2] = temdata[0];
                outData[i * 2 + 1] = temdata[1];
            }
            return outData;
        }

        ///<summary>音频字节转音频</summary>
        public static void BytesToAudioClip(this byte[] data , string clipName , out AudioClip clip)
        {
            float[] clipData = new float[data.Length / 2];
            for(int i = 0; i < clipData.Length; i++)
            {
                clipData[i / 2] = BytesToFloat(data[i * 2] , data[i * 2 + 1]);
            }
            clip = AudioClip.Create(clipName , 16000 * 10 , 1 , 16000 , false);
            clip.SetData(clipData , 0);
        }

        private static float BytesToFloat(byte firstBytes , byte secondBytes)
        {
            return ( BitConverter.IsLittleEndian ? (short)( ( secondBytes << 8 ) | firstBytes ) : (short)( ( firstBytes << 8 ) | secondBytes ) ) / 32768.0F;
        }


    }
    /// <summary>
    /// list扩展
    /// </summary>
    public static class BuiltinRuntimeListExtend
    {
        /// <summary>升序排列</summary>
        public static void AscendingSort(this List<int> list)
        {
            list.Sort((x , y) => { return x.CompareTo(y); });
        }

        /// <summary>升序排列</summary>
        public static void AscendingSort(this List<float> list)
        {
            list.Sort((x , y) => { return x.CompareTo(y); });
        }

        /// <summary>升序排列</summary>
        public static void AscendingSort(this List<string> list)
        {
            list.Sort((x , y) => { return x.CompareTo(y); });
        }

        /// <summary>降序排列</summary>
        public static void DescendingSort(this List<int> list)
        {
            list.Sort((x , y) => { return -x.CompareTo(y); });
        }

        /// <summary>降序排列</summary>
        public static void DescendingSort(this List<float> list)
        {
            list.Sort((x , y) => { return -x.CompareTo(y); });
        }

        /// <summary>降序排列</summary>
        public static void DescendingSort(this List<string> list)
        {
            list.Sort((x , y) => { return -x.CompareTo(y); });
        }

        ///<summary>获取list内某元素的数量</summary>
        ///<param name="match">匹配条件</param>
        public static int GetElementCount<T>(this List<T> list , Predicate<T> match)
        {
            return list.FindAll(match).Count;
        }
    }

    /// <summary>
    /// string扩展
    /// </summary>
    public static class BuiltinRuntimeStringExtend
    {
        ///<summary>string 转 int</summary>
		public static int ToInt(this string str)
        {
            return int.Parse(str);
        }

        ///<summary>string 转 int</summary>
        public static int TryToInt(this string str)
        {
            int.TryParse(str , out int result);
            return result;
        }
        public static void TryToInt(this string str , out int result)
        {
            int.TryParse(str , out result);
        }

        ///<summary>字符串拼接</summary>
        public static string StringJoint(this string str , string str1)
        {
            StringBuilder sb = new StringBuilder(str);
            sb.Append(str1);
            return sb.ToString( );
        }

        ///<summary>字符串是否为空</summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        ///<summary>字符串值是否为空/空格。</summary>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        ///<summary>大写第一个字符</summary>
        public static string UppercaseFirst(this string str)
        {
            return char.ToUpper(str[0]) + str[1..];
        }

        ///<summary>小写第一个字符</summary>
        public static string LowercaseFirst(this string str)
        {
            return char.ToLower(str[0]) + str[1..];
        }

        ///<summary>字符串内是否包含中文</summary>
        public static bool ContainCinese(this string str)
        {
            return Regex.IsMatch(str , @"[\u4e00-\u9fa5]");
        }

        ///<summary>字符串值是有效的Email</summary>
        public static bool IsValidEmail(this string str)
        {
            return Regex.IsMatch(str , @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        ///<summary>字符串是否是有效手机号</summary>
        public static bool IsValidMobilePhoneNumber(this string str)
        {
            return Regex.IsMatch(str , @"^0{0,1}(13[4-9]|15[7-9]|15[0-2]|18[7-8])[0-9]{8}$");
        }
        ///<summary>使用md5加密字符</summary>
		public static string MD5EncryptString(this string str)
        {
            MD5 md5 = MD5.Create( );
            byte[] byteOld = Encoding.UTF8.GetBytes(str);
            byte[] byteName = md5.ComputeHash(byteOld);
            StringBuilder sb = new StringBuilder( );
            foreach(byte b in byteName)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString( );
        }

        public static bool SafeStartsWith(this string check , string starter)
        {
            if(check == null || starter == null)
            {
                return false;
            }
            if(check.Length < starter.Length)
            {
                return false;
            }
            for(int i = 0; i < check.Length; i++)
            {
                if(check[i] != starter[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
    /// <summary>
    /// Componet扩展
    /// </summary>
    public static class BuiltinRuntimeComponetExtend
    {
        ///<summary>获取组件，不存在则添加</summary>
		public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            if(go.GetComponent<T>( ) == null)
            {
                go.AddComponent<T>( );
            }
            return go.GetComponent<T>( );
        }

        ///<summary>物体是否存在该组件</summary>
        public static bool HasComponet<T>(this GameObject go) where T : Component => go.GetComponent<T>( ) != null;

        public static T GetOrAddCompoentOrName<T>(this GameObject go , string name) where T : Component
        {
            go.name = name;
            return go.GetComponent<T>( ) ?? go.AddComponent<T>( );
        }

        /// <summary>移除物体上挂载的组件</summary>
        public static void Remove<T>(this GameObject go) where T : MonoBehaviour
        {
            if(go.HasComponet<T>( ))
            {
                GameObject.Destroy(go.GetComponent<T>( ));
            }
        }

        ///<summary>改变image透明度</summary>
        public static void ChangeAlpha(this Image image , float alpha)
        {
            Color oldcolor = image.color;
            image.color = new Color(oldcolor.r , oldcolor.g , oldcolor.b , alpha);
        }

        /// <summary>改变对象的层级</summary>
        public static void ChangeLayer(this GameObject go , string layerName)
        {
            int layer = LayerMask.NameToLayer(layerName);
            if(go)
            {
                foreach(Transform item in go.GetComponentInChildren<Transform>(true))
                {
                    item.gameObject.layer = layer;
                }
            }
        }
        /// <summary>起协程,先等待延迟，然后运行事件</summary>
		public static Coroutine WaitSomeTime(this MonoBehaviour mono , float time , UnityEngine.Events.UnityAction action)
        {
            return mono.StartCoroutine(WaitSomeTime(action , time));
        }

        private static IEnumerator WaitSomeTime(UnityEngine.Events.UnityAction action , float time)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke( );
        }
    }

    /// <summary>
    /// DateTime扩展
    /// </summary>
    public static class BuiltinRuntimeDateTimeExtend
    {
        /// <summary>
		/// 获取时间戳
		/// </summary>
		/// <param name="time">时间</param>
		/// <returns>时间戳</returns>
		public static double GetTimeStamp(this DateTime time)
        {
            return ( time - new DateTime(1970 , 1 , 1 , 0 , 0 , 0 , 0) ).TotalSeconds;
        }
        /// <summary>
		/// 距离今天结束还有多少秒.
		/// </summary>
		/// <returns>今天剩余秒数</returns>
		public static int TodayTimeRemainingSecond( )
        {
            return ( ( 23 - DateTime.Now.Hour ) * 60 + 59 - DateTime.Now.Minute ) * 60 + 60 - DateTime.Now.Second;
        }
        /// <summary>
        /// 转为中文(周*)
        /// </summary>
        /// <param name="week"></param>
        /// <returns></returns>
        public static string ToWeek(this DayOfWeek week)
        {
            return week switch
            {
                DayOfWeek.Monday => "周一",
                DayOfWeek.Friday => "周五",
                DayOfWeek.Saturday => "周六",
                DayOfWeek.Sunday => "周日",
                DayOfWeek.Thursday => "周二",
                DayOfWeek.Tuesday => "周四",
                DayOfWeek.Wednesday => "周三",
                _ => null,
            };
        }

        ///<summary>转为中文 (星期*)</summary>
        public static string ToWeeks(this DayOfWeek week)
        {
            return week switch
            {
                DayOfWeek.Monday => "星期一",
                DayOfWeek.Friday => "星期五",
                DayOfWeek.Saturday => "星期六",
                DayOfWeek.Sunday => "星期日",
                DayOfWeek.Thursday => "星期二",
                DayOfWeek.Tuesday => "星期四",
                DayOfWeek.Wednesday => "星期三",
                _ => null,
            };
        }

        /// <summary>
        /// 转换格式 yyyy-MM-dd
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToDateString_(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 转换格式 yyyy/MM/dd
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime time)
        {
            return time.ToString("yyyy/MM/dd");
        }

        /// <summary>
        /// 转换格式 yyyy年MM月dd
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToDateStrings(this DateTime time)
        {
            return time.ToString("yyyy年MM月dd日");
        }
        /// <summary>
        /// 转换格式 yyyy/MM/dd HH:mm:ss
        /// </summary>
        /// <param name="self"></param>
        /// <returns>yyyy/MM/dd HH:mm:ss</returns>
        public static string ToDateTimeString(this DateTime self)
        {
            return self.ToString("yyyy/MM/dd HH:mm:ss");
        }
        /// <summary>
        /// 转换格式 yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="self"></param>
        /// <returns>yyyy-MM-dd HH:mm:ss</returns>
        public static string ToDateTimeString2(this DateTime self)
        {
            return self.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 转换格式 yyyy年MM月dd日 HH:mm:ss
        /// </summary>
        /// <param name="self"></param>
        /// <returns>yyyy年MM月dd日 HH:mm:ss</returns>
        public static string ToDateTimeString3(this DateTime self)
        {
            return self.ToString("yyyy年MM月dd日 HH:mm:ss");
        }

        /// <summary>
        /// 转换时间格式
        /// <para>使用:2020-1-2：yyyy年MM月dd日；结果:2020年1月2日</para>
        /// </summary>
        public static string TimeStringToFromat(this string deteTimeString , string newDataTimeFormat) => DateTime.Parse(deteTimeString).ToString(newDataTimeFormat);

    }

    /// <summary>
    /// Ray扩展
    /// </summary>
    public static class BuiltinRuntimeRayExtend
    {
        /// <summary>
		/// 获取检测到的物体
		/// </summary>
		/// <param name="ray">射线</param>
		/// <returns>检测到的物体</returns>
		public static GameObject GetDetectedObject(this Ray ray)
        {
            return Physics.Raycast(ray , out RaycastHit hit) ? hit.collider.gameObject : null;
        }

        /// <summary>
        /// 获取检测到的物体
        /// </summary>
        /// <param name="ray">射线</param>
        /// <param name="maxDistance">检测的最大距离</param>
        /// <returns>检测到的物体</returns>
        public static GameObject GetDetectedObject(this Ray ray , float maxDistance)
        {
            return Physics.Raycast(ray , out RaycastHit hit , maxDistance) ? hit.collider.gameObject : null;
        }

        /// <summary>
        /// 获取检测到的物体
        /// </summary>
        /// <param name="ray">射线</param>
        /// <param name="maxDistance">检测的最大距离</param>
        /// <param name="layerMask">检测的层级</param>
        /// <returns>检测到的物体</returns>
        public static GameObject GetDetectedObject(this Ray ray , float maxDistance , int layerMask)
        {
            return Physics.Raycast(ray , out RaycastHit hit , maxDistance , layerMask) ? hit.collider.gameObject : null;
        }

        /// <summary>
        /// 判断是否检测到目标物体
        /// </summary>
        /// <param name="ray">射线</param>
        /// <param name="target">目标物体</param>
        /// <returns>若射线检测到目标物体返回true,否则返回false</returns>
        public static bool IsDetectedGameObject(this Ray ray , GameObject target)
        {
            return Physics.Raycast(ray , out RaycastHit hit) && hit.collider.gameObject == target;
        }

        /// <summary>
        /// 判断是否检测到目标物体
        /// </summary>
        /// <param name="ray">射线</param>
        /// <param name="maxDistance">检测最大距离</param>
        /// <param name="target">目前物体</param>
        /// <returns>若射线检测到目标物体返回true,否则返回false</returns>
        public static bool IsDetectedGameObject(this Ray ray , float maxDistance , GameObject target)
        {
            return Physics.Raycast(ray , out RaycastHit hit , maxDistance) && hit.collider.gameObject == target;
        }

        /// <summary>
        /// 判断是否检测到目标物体
        /// </summary>
        /// <param name="ray">射线</param>
        /// <param name="maxDistance">检测最大距离</param>
        /// <param name="layerMask">检测的层级</param>
        /// <param name="target">目前物体</param>
        /// <returns>若射线检测到目标物体返回true,否则返回false</returns>
        public static bool IsDetectedGameObject(this Ray ray , float maxDistance , int layerMask , GameObject target)
        {
            return Physics.Raycast(ray , out RaycastHit hit , maxDistance , layerMask) && hit.collider.gameObject == target;
        }

        /// <summary>
        /// 判断是否检测到目标组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="ray">射线</param>
        /// <param name="t">组件</param>
        /// <returns>若射线检测到目标组件返回true,否则返回false</returns>
        public static bool IsDetectedComponent<T>(this Ray ray , out T t) where T : Component
        {
            if(Physics.Raycast(ray , out RaycastHit hit))
            {
                t = hit.collider.GetComponent<T>( );
                return t != null;
            }
            t = null;
            return false;
        }

        /// <summary>
        /// 判断是否检测到目标组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="ray">射线</param>
        /// <param name="maxDistance">检测最大距离</param>
        /// <param name="t">组件</param>
        /// <returns>若射线检测到目标组件返回true,否则返回false</returns>
        public static bool IsDetectedComponent<T>(this Ray ray , float maxDistance , out T t) where T : Component
        {
            if(Physics.Raycast(ray , out RaycastHit hit , maxDistance))
            {
                t = hit.collider.GetComponent<T>( );
                return t != null;
            }
            t = null;
            return false;
        }

        /// <summary>
        /// 判断是否检测到目标组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="ray">射线</param>
        /// <param name="maxDistance">检测最大距离</param>
        /// <param name="layerMask">检测的层级</param>
        /// <param name="t">组件</param>
        /// <returns>若射线检测到目标组件返回true,否则返回false</returns>
        public static bool IsDetectedComponent<T>(this Ray ray , float maxDistance , int layerMask , out T t) where T : Component
        {
            if(Physics.Raycast(ray , out RaycastHit hit , maxDistance , layerMask))
            {
                t = hit.collider.GetComponent<T>( );
                return t != null;
            }
            t = null;
            return false;
        }

    }

    /// <summary>
    /// Graphic扩展
    /// </summary>
    public static class BuiltinRuntimeGraphicExtend
    {
        /// <summary>
		/// 设置颜色值
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="self">实例</param>
		/// <param name="color">颜色值</param>
		/// <returns>实例</returns>
		public static T SetColor<T>(this T self , Color color) where T : Graphic
        {
            self.color = color;
            return self;
        }

        /// <summary>
        /// 设置颜色值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="self">实例</param>
        /// <param name="r">颜色r值</param>
        /// <param name="g">颜色g值</param>
        /// <param name="b">颜色b值</param>
        /// <returns>实例</returns>
        public static T SetColor<T>(this T self , float r , float g , float b) where T : Graphic
        {
            Color color = self.color;
            color.r = r;
            color.g = g;
            color.b = b;
            self.color = color;
            return self;
        }

        /// <summary>
        /// 设置颜色值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="self">实例</param>
        /// <param name="r">颜色r值</param>
        /// <param name="g">颜色g值</param>
        /// <param name="b">颜色b值</param>
        /// <param name="a">颜色a值</param>
        /// <returns>实例</returns>
        public static T SetColor<T>(this T self , float r , float g , float b , float a) where T : Graphic
        {
            Color color = self.color;
            color.r = r;
            color.g = g;
            color.b = b;
            color.a = a;
            self.color = color;
            return self;
        }

        /// <summary>
        /// 设置颜色Alpha值
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="self">实例</param>
        /// <param name="alpha">alpha值</param>
        /// <returns>实例</returns>
        public static T SetColorAlpha<T>(this T self , float alpha) where T : Graphic
        {
            Color color = self.color;
            color.a = alpha;
            self.color = color;
            return self;
        }

        /// <summary>
        /// 设置材质球
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="self">实例</param>
        /// <param name="material">材质球</param>
        /// <returns>实例</returns>
        public static T SetMaterial<T>(this T self , Material material) where T : Graphic
        {
            self.material = material;
            return self;
        }

        /// <summary>
        /// 设置RaycastTarget属性
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="self">实例</param>
        /// <param name="raycastTarget">raycast target</param>
        /// <returns>实例</returns>
        public static T SetRaycastTarget<T>(this T self , bool raycastTarget) where T : Graphic
        {
            self.raycastTarget = raycastTarget;
            return self;
        }
    }

    /// <summary>
    /// Vector扩展
    /// </summary>
    public static class BuiltinRuntimeVectorExtend
    {
        public static float[] ToArray(this Vector3 vector3)
        {
            float[] array = new float[3];
            array[0] = vector3.x;
            array[1] = vector3.y;
            array[2] = vector3.z;
            return array;
        }

        public static Quaternion ToQuaternion(this Vector3 vector3)
        {
            return Quaternion.Euler(vector3);
        }

        /// <summary>
		/// 2D叉乘
		/// </summary>
		/// <param name="v1">点1</param>
		/// <param name="v2">点2</param>
		/// <returns></returns>
		public static float CrossProduct2D(this Vector2 v1 , Vector2 v2)
        {
            //叉乘运算公式 x1*y2 - x2*y1
            return v1.x * v2.y - v2.x * v1.y;
        }

        /// <summary>
        /// 点是否在直线上
        /// </summary>
        /// <param name="point">点</param>
        /// <param name="lineStart">线的开始点</param>
        /// <param name="lineEnd">线的结束点</param>
        /// <returns></returns>
        public static bool IsPointOnLine(this Vector2 point , Vector2 lineStart , Vector2 lineEnd)
        {
            float value = CrossProduct2D(point - lineStart , lineEnd - lineStart);
            /* 使用 Mathf.Approximately(value,0) 方式，在斜线上好像无法趋近为0*/
            return Math.Abs(value) < 0.003;
        }

        /// <summary>
        /// 点是否在线段上
        /// </summary>
        /// <param name="point"></param>
        /// <param name="lineStart"></param>
        /// <param name="lineEnd"></param>
        /// <returns></returns>
        public static bool IsPointOnSegment(this Vector2 point , Vector2 lineStart , Vector2 lineEnd)
        {
            //1.先通过向量的叉乘确定点是否在直线上
            //2.在拍段点是否在指定线段的矩形范围内
            if(IsPointOnLine(point , lineStart , lineEnd))
            {
                //点的x值大于最小，小于最大x值 以及y值大于最小，小于最大
                if(point.x >= Mathf.Min(lineStart.x , lineEnd.x) && point.x <= Mathf.Max(lineStart.x , lineEnd.x) &&
                    point.y >= Mathf.Min(lineStart.y , lineEnd.y) && point.y <= Mathf.Max(lineStart.y , lineEnd.y))
                    return true;
            }
            return false;
        }


        public static List<Vector3> GetMain(this List<Vector3> vector3 , out Vector3 min)
        {
            min = vector3[0];
            for(int i = 0; i < vector3.Count; i++)
            {
                min = Vector3.Min(min , vector3[i]);
            }
            return vector3;
        }

        public static List<Vector3> GetMax(this List<Vector3> vector3 , out Vector3 max)
        {
            max = vector3[0];

            for(int i = 0; i < vector3.Count; i++)
            {
                max = Vector3.Max(max , vector3[i]);
            }
            return vector3;
        }

        /// <summary>
        /// 一个点绕另一个点旋转指定度数,并返回旋转后的点
        /// </summary>
        /// <param name="pos">需要旋转的点</param>
        /// <param name="center">围绕旋转的点</param>
        /// <param name="axis">旋转轴方向</param>
        /// <param name="angle">旋转角度</param>
        /// <returns>旋转后得到的点</returns>
        public static Vector3 RotateRound(this Vector3 pos , Vector3 center , Vector3 axis , float angle)
        {
            return center + ( Quaternion.AngleAxis(angle , axis) * ( pos - center ) );
        }

        /// <summary>
        /// 计算角度-叉乘
        /// </summary>
        /// <param name="vector3">三维向量</param>
        /// <param name="target">目标三维向量</param>
        /// <returns>角度值</returns>
        public static float GetAngle(this Vector3 vector3 , Vector3 target)
        {
            return Mathf.Acos(Vector3.Dot(vector3.normalized , target.normalized)) * Mathf.Rad2Deg;
        }

        /// <summary>
		/// 生成贝塞尔曲线
		/// </summary>
		/// <param name="self">控制点</param>
		/// <param name="startPoint">贝塞尔曲线起点</param>
		/// <param name="endPoint">贝塞尔曲线终点</param>
		/// <param name="count">贝塞尔曲线点个数</param>
		/// <returns>组成贝塞尔曲线的点集合</returns>
		public static Vector3[] GenerateBeizer(this Vector3 self , Vector3 startPoint , Vector3 endPoint , int count)
        {
            Vector3[] retValue = new Vector3[count];
            for(int i = 1; i <= count; i++)
            {
                float t = i / (float)count;
                float u = 1 - t;
                float tt = Mathf.Pow(t , 2);
                float uu = Mathf.Pow(u , 2);
                Vector3 point = uu * startPoint;
                point += 2 * u * t * self;
                point += tt * endPoint;
                retValue[i - 1] = point;
            }
            return retValue;
        }

        /// <summary>
        /// 与目标点的距离
        /// </summary>
        /// <param name="self">Vector3</param>
        /// <param name="target">目标点</param>
        /// <returns>距离</returns>
        public static float Distance(this Vector3 self , Vector3 target)
        {
            return Vector3.Distance(self , target);
        }

        /// <summary>
        /// 与目标点的中点
        /// </summary>
        /// <param name="self">Vector3</param>
        /// <param name="target">目标点</param>
        /// <returns>中点</returns>
        public static Vector3 Half(this Vector3 self , Vector3 target)
        {
            return ( self + target ) / 2.0f;
        }

        /// <summary>
        /// 指向目标点的方向
        /// </summary>
        /// <param name="self">Vector3</param>
        /// <param name="target">目标点</param>
        /// <returns>方向</returns>
        public static Vector3 Direction(this Vector3 self , Vector3 target)
        {
            return ( target - self ).normalized;
        }

        /// <summary>
        /// 判断目标点是否在指定区域内
        /// </summary>
        /// <param name="self">目标点</param>
        /// <param name="points">区域各顶点</param>
        /// <param name="height">区域高度</param>
        /// <returns>是否在区域内</returns>
        public static bool IsInRange(this Vector3 self , Vector3[] points , float height)
        {
            if(self.y > height || self.y < -height) return false;
            var comparePoint = ( points[0] + points[1] ) * 0.5f;
            comparePoint += ( comparePoint - self ).normalized * 10000;
            int count = 0;
            for(int i = 0; i < points.Length; i++)
            {
                var a = points[i % points.Length];
                var b = points[( i + 1 ) % points.Length];
                var crossA = Mathf.Sign(Vector3.Cross(comparePoint - self , a - self).y);
                var crossB = Mathf.Sign(Vector3.Cross(comparePoint - self , b - self).y);
                if(Mathf.Approximately(crossA , crossB)) continue;
                var crossC = Mathf.Sign(Vector3.Cross(b - a , self - a).y);
                var crossD = Mathf.Sign(Vector3.Cross(b - a , comparePoint - a).y);
                if(Mathf.Approximately(crossC , crossD)) continue;
                count++;
            }
            return count % 2 == 1;
        }

        /// <summary>
        /// 获取点坐标数组
        /// </summary>
        /// <param name="self">点列表</param>
        /// <returns>点坐标数组</returns>
        public static Vector3[] GetPositions(this List<Transform> self)
        {
            Vector3[] retArray = new Vector3[self.Count];
            for(int i = 0; i < self.Count; i++)
            {
                retArray[i] = self[i].position;
            }
            return retArray;
        }

        /// <summary>
        /// 获取点坐标数组
        /// </summary>
        /// <param name="self">点数组</param>
        /// <returns>点坐标数组</returns>
        public static Vector3[] GetPositions(this Transform[] self)
        {
            Vector3[] retArray = new Vector3[self.Length];
            for(int i = 0; i < self.Length; i++)
            {
                retArray[i] = self[i].position;
            }
            return retArray;
        }

    }

    /// <summary>
    /// LineRenderer扩展
    /// </summary>
    public static class BuiltinRuntimeLineRendererExtend
    {
        /// <summary>
		/// 设置起始宽度
		/// </summary>
		/// <param name="self">光线渲染器</param>
		/// <param name="width">起始宽度</param>
		/// <returns>光线渲染器</returns>
		public static LineRenderer SetStartWidth(this LineRenderer self , float width)
        {
            self.startWidth = width;
            return self;
        }

        /// <summary>
        /// 设置结束宽度
        /// </summary>
        /// <param name="self">光线渲染器</param>
        /// <param name="width">结束宽度</param>
        /// <returns>光线渲染器</returns>
        public static LineRenderer SetEndWidth(this LineRenderer self , float width)
        {
            self.endWidth = width;
            return self;
        }

        /// <summary>
        /// 设置起始颜色
        /// </summary>
        /// <param name="self">光线渲染器</param>
        /// <param name="color">起始颜色</param>
        /// <returns>光线渲染器</returns>
        public static LineRenderer SetStartColor(this LineRenderer self , Color color)
        {
            self.startColor = color;
            return self;
        }

        /// <summary>
        /// 设置结束颜色
        /// </summary>
        /// <param name="self">光线渲染器</param>
        /// <param name="color">结束颜色</param>
        /// <returns>光线渲染器</returns>
        public static LineRenderer SetEndColor(this LineRenderer self , Color color)
        {
            self.endColor = color;
            return self;
        }

        /// <summary>
        /// 设置点个数
        /// </summary>
        /// <param name="self">光线渲染器</param>
        /// <param name="count">点个数</param>
        /// <returns>光线渲染器</returns>
        public static LineRenderer SetPositionCount(this LineRenderer self , int count)
        {
            self.positionCount = count;
            return self;
        }

        /// <summary>
        /// 设置点位置
        /// </summary>
        /// <param name="self">光线渲染器</param>
        /// <param name="index">索引</param>
        /// <param name="position">位置</param>
        /// <returns>光线渲染器</returns>
        public static LineRenderer SetLinePosition(this LineRenderer self , int index , Vector3 position)
        {
            self.SetPosition(index , position);
            return self;
        }

        /// <summary>
        /// 设置是否循环（终点是否连接起点）
        /// </summary>
        /// <param name="self">光线渲染器</param>
        /// <param name="loop">是否循环</param>
        /// <returns>光线渲染器</returns>
        public static LineRenderer SetLoop(this LineRenderer self , bool loop)
        {
            self.loop = loop;
            return self;
        }

        /// <summary>
        /// 设置CornerVertices属性
        /// </summary>
        /// <param name="self">光线渲染器</param>
        /// <param name="cornerVertices">conner vertices</param>
        /// <returns>光线渲染器</returns>
        public static LineRenderer SetCornerVertices(this LineRenderer self , int cornerVertices)
        {
            self.numCornerVertices = cornerVertices;
            return self;
        }

        /// <summary>
        /// 设置EndCapVertices属性
        /// </summary>
        /// <param name="self">光线渲染器</param>
        /// <param name="endCapVertices">end cap vertices</param>
        /// <returns>光线渲染器</returns>
        public static LineRenderer SetEndCapVertices(this LineRenderer self , int endCapVertices)
        {
            self.numCapVertices = endCapVertices;
            return self;
        }

        /// <summary>
        /// 设置Alignment属性
        /// </summary>
        /// <param name="self">光线渲染器</param>
        /// <param name="alignment">alignment</param>
        /// <returns>光线渲染器</returns>
        public static LineRenderer SetAlignment(this LineRenderer self , LineAlignment alignment)
        {
            self.alignment = alignment;
            return self;
        }

        /// <summary>
        /// 设置TextureMode
        /// </summary>
        /// <param name="self">光线渲染器</param>
        /// <param name="textureMode">texture mode</param>
        /// <returns>光线渲染器</returns>
        public static LineRenderer SetTextureMode(this LineRenderer self , LineTextureMode textureMode)
        {
            self.textureMode = textureMode;
            return self;
        }

        /// <summary>
        /// 设置材质球
        /// </summary>
        /// <param name="self">光线渲染器</param>
        /// <param name="material">材质球</param>
        /// <returns>光线渲染器</returns>
        public static LineRenderer SetMaterial(this LineRenderer self , Material material)
        {
            self.material = material;
            return self;
        }

    }

    /// <summary>
    /// Math扩展
    /// </summary>
    public static class BuiltinRuntimeMathExtend
    {
        /// <summary>
		/// 保留小数指定位数
		/// </summary>
		/// <param name="self">float值</param>
		/// <param name="point">保留位置</param>
		/// <returns>保留指定小数位数后的float值</returns>
		public static float Round(this float self , int point)
        {
            int scale = 1;
            for(int i = 0; i < point; i++)
            {
                scale *= 10;
            }
            self *= scale;
            return Mathf.Round(self) / scale;
        }

        /// <summary>
        /// 判断是否约等于目标值
        /// </summary>
        /// <param name="self">float值</param>
        /// <param name="targetValue">目标值</param>
        /// <returns>若约等于则返回true,否则返回false</returns>
        public static bool IsApproximately(this float self , float targetValue)
        {
            return Mathf.Approximately(self , targetValue);
        }

        /// <summary>
        /// 平方和根
        /// </summary>
        /// <param name="x">int值</param>
        /// <param name="y">int值</param>
        /// <returns>x与y的平方和根</returns>
        public static float Sqrt(this int x , int y)
        {
            int n2 = x ^ 2 + y ^ 2;
            return Mathf.Sqrt(n2);
        }

        /// <summary>
		/// 计算多边形周长
		/// </summary>
		/// <param name="self">多边形顶点数组</param>
		/// <returns>周长</returns>
		public static float GetPolygonPerimeter(this Vector3[] self)
        {
            if(self.Length < 3) return 0.0f;
            float retV = 0f;
            for(int i = 0; i < self.Length; i++)
            {
                retV += Vector3.Distance(self[i] , self[( i + 1 < self.Length ? i + 1 : 0 )]);
            }
            return retV;
        }

        /// <summary>
        /// 计算多边形周长
        /// </summary>
        /// <param name="self">多边形顶点列表</param>
        /// <returns>周长</returns>
        public static float GetPolygonPerimeter(this List<Vector3> self)
        {
            if(self.Count < 3) return 0.0f;
            float retV = 0f;
            for(int i = 0; i < self.Count; i++)
            {
                retV += Vector3.Distance(self[i] , self[( i + 1 < self.Count ? i + 1 : 0 )]);
            }
            return retV;
        }

        /// <summary>
        /// 计算多边形面积
        /// </summary>
        /// <param name="self">多边形顶点数组</param>
        /// <returns>面积</returns>
        public static float GetPolygonArea(this Vector3[] self)
        {
            if(self.Length < 3) return 0.0f;
            float retV = self[0].z * ( self[^1].x - self[1].x );
            for(int i = 1; i < self.Length; i++)
            {
                retV += self[i].z * ( self[i - 1].x - self[( i + 1 ) % self.Length].x );
            }
            return Mathf.Abs(retV / 2.0f);
        }

        /// <summary>
        /// 计算多边形面积
        /// </summary>
        /// <param name="self">多边形顶点列表</param>
        /// <returns>面积</returns>
        public static float GetPolygonArea(this List<Vector3> self)
        {
            if(self.Count < 3) return 0.0f;
            float retV = self[0].z * ( self[^1].x - self[1].x );
            for(int i = 1; i < self.Count; i++)
            {
                retV += self[i].z * ( self[i - 1].x - self[( i + 1 ) % self.Count].x );
            }
            return Mathf.Abs(retV / 2.0f);
        }

        /// <summary>
        /// 计算圆的周长
        /// </summary>
        /// <param name="self">半径</param>
        /// <returns>周长</returns>
        public static float GetCirclePerimeter(this float self)
        {
            return Mathf.PI * 2f * self;
        }

        /// <summary>
        /// 计算圆的面积
        /// </summary>
        /// <param name="self">半径</param>
        /// <returns>面积</returns>
        public static float GetCircleArea(this float self)
        {
            return Mathf.PI * Mathf.Pow(self , 2);
        }

        /// <summary>
        /// 三角函数计算对边的长度
        /// </summary>
        /// <param name="self">角度</param>
        /// <param name="neighbouringSideLength">邻边的长度</param>
        /// <returns>对边的长度</returns>
        public static float GetFaceSideLength(this float self , float neighbouringSideLength)
        {
            return neighbouringSideLength * Mathf.Tan(self * Mathf.Deg2Rad);
        }

        /// <summary>
        /// 三角函数计算邻边的长度
        /// </summary>
        /// <param name="self">角度</param>
        /// <param name="faceSideLength">对边的长度</param>
        /// <returns>邻边的长度</returns>
        public static float GetNeighbouringSideLength(this float self , float faceSideLength)
        {
            return faceSideLength / Mathf.Tan(self * Mathf.Deg2Rad);
        }

        /// <summary>
        /// 勾股定理计算斜边的长度
        /// </summary>
        /// <param name="self">直角边的长度</param>
        /// <param name="anotherRightangleSideLength">另一条直角边的长度</param>
        /// <returns>斜边的长度</returns>
        public static float GetHypotenuseLength(this float self , float anotherRightangleSideLength)
        {
            return Mathf.Sqrt(Mathf.Pow(self , 2f) + Mathf.Pow(anotherRightangleSideLength , 2f));
        }

        /// <summary>
        /// 正弦
        /// </summary>
        /// <param name="self">角度</param>
        /// <returns>正弦值</returns>
        public static float Sin(this float self)
        {
            return Mathf.Sin(self * Mathf.Deg2Rad);
        }

        /// <summary>
        /// 余弦
        /// </summary>
        /// <param name="self">角度</param>
        /// <returns></returns>
        public static float Cos(this float self)
        {
            return Mathf.Cos(self * Mathf.Deg2Rad);
        }

        /// <summary>
        /// 正切
        /// </summary>
        /// <param name="self">角度</param>
        /// <returns>正切值</returns>
        public static float Tan(this float self)
        {
            return Mathf.Tan(self * Mathf.Deg2Rad);
        }

        /// <summary>
        /// 反正弦
        /// </summary>
        /// <param name="self">正弦值</param>
        /// <returns>角度</returns>
        public static float ArcSin(this float self)
        {
            return Mathf.Asin(self) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// 反余弦
        /// </summary>
        /// <param name="self">余弦值</param>
        /// <returns>角度</returns>
        public static float ArcCos(this float self)
        {
            return Mathf.Acos(self) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// 反正切
        /// </summary>
        /// <param name="self">正切值</param>
        /// <returns>角度</returns>
        public static float ArcTan(this float self)
        {
            return Mathf.Atan(self) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// 度转弧度
        /// </summary>
        /// <param name="self">度</param>
        /// <returns>弧度</returns>
        public static float Deg2Rad(this float self)
        {
            return self * Mathf.Deg2Rad;
        }

        /// <summary>
        /// 弧度转度
        /// </summary>
        /// <param name="self">弧度</param>
        /// <returns>度</returns>
        public static float Rad2Deg(this float self)
        {
            return self * Mathf.Rad2Deg;
        }
    }

    /// <summary>
    /// 贝塞尔曲线
    /// </summary>
    public static class BuiltinRuntimeBezier
    {
        /// <summary>
		/// 线性
		/// </summary>
		/// <param name="p0">开始点</param>
		/// <param name="p1">终点</param>
		/// <param name="t">0-1</param>
		/// <returns></returns>
		public static Vector3 BezierPoint(Vector3 p0 , Vector3 p1 , float t)
        {
            return ( ( 1 - t ) * p0 ) + t * p1;
        }

        /// <summary>
        /// 二阶贝塞尔曲线
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 BezierPoint(Vector3 p0 , Vector3 p1 , Vector3 p2 , float t)
        {
            Vector3 p0p1 = ( 1 - t ) * p0 + t * p1;
            Vector3 p1p2 = ( 1 - t ) * p1 + t * p2;
            Vector3 result = ( 1 - t ) * p0p1 + t * p1p2;
            return result;
        }

        /// <summary>
        /// 三阶曲线
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Vector3 BezierPoint(Vector3 p0 , Vector3 p1 , Vector3 p2 , Vector3 p3 , float t)
        {
            Vector3 result;
            Vector3 p0p1 = ( 1 - t ) * p0 + t * p1;
            Vector3 p1p2 = ( 1 - t ) * p1 + t * p2;
            Vector3 p2p3 = ( 1 - t ) * p2 + t * p3;
            Vector3 p0p1p2 = ( 1 - t ) * p0p1 + t * p1p2;
            Vector3 p1p2p3 = ( 1 - t ) * p1p2 + t * p2p3;
            result = ( 1 - t ) * p0p1p2 + t * p1p2p3;
            return result;
        }

        /// <summary>
        /// 多阶曲线  （可以递归 有多组线性组合）
        /// </summary>
        /// <param name="t"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Vector3 BezierPoint(float t , List<Vector3> p)
        {
            if(p.Count < 2)
                return p[0];
            List<Vector3> newP = new List<Vector3>( );
            for(int i = 0; i < p.Count - 1; i++)
            {
                Vector3 p0p1 = ( 1 - t ) * p[i] + t * p[i + 1];
                newP.Add(p0p1);
            }
            return BezierPoint(t , newP);
        }

        /// <summary>
        /// 获取存储贝塞尔曲线点的数组(二阶)
        /// </summary>
        /// <param name="startPoint">起始点</param>
        /// <param name="controlPoint">控制点</param>
        /// <param name="endPoint">目标点</param>
        /// <param name="segmentNum">采样点的数量</param>
        /// <returns>存储贝塞尔曲线点的数组</returns>
        public static Vector3[] GetBeizerPointList(Vector3 startPoint , Vector3 controlPoint , Vector3 endPoint , int segmentNum)
        {
            Vector3[] path = new Vector3[segmentNum];
            for(int i = 1; i <= segmentNum; i++)
            {
                float t = i / (float)segmentNum;
                Vector3 pixel = BezierPoint(startPoint , controlPoint , endPoint , t);
                path[i - 1] = pixel;
            }
            return path;
        }

        /// <summary>
        /// 获取存储贝塞尔曲线点的数组(多阶)
        /// </summary>
        /// <param name="segmentNum">采样点的数量</param>
        /// <param name="p">控制点集合</param>
        /// <returns></returns>
        public static Vector3[] GetBeizerPointList(int segmentNum , List<Vector3> p)
        {
            Vector3[] path = new Vector3[segmentNum];
            for(int i = 1; i <= segmentNum; i++)
            {
                float t = i / (float)segmentNum;
                Vector3 pixel = BezierPoint(t , p);
                path[i - 1] = pixel;
            }
            return path;
        }
    }

    /// <summary>
    /// 外部程序
    /// </summary>
    public static class BuiltinRuntimeProcess
    {
        /// <summary>
        /// 打开一个windows程序
        /// </summary>
        /// <param name="path">程序路径</param>
        /// <param name="process">程序</param>
        public static void OpenWindowsApp(string path , out Process process)
        {
            process = new Process( );
            process.StartInfo.FileName = path;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start( );
        }
        /// <summary>
        /// 结束一个windows程序
        /// </summary>
        /// <param name="process">程序</param>
        public static void KillWindowsApp(this Process process)
        {
            process?.Kill( );
        }

    }
}
