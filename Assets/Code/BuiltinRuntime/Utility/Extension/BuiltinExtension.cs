using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using System.IO;

namespace UGHGame.BuiltinRuntime
{
    /// <summary>
    /// UI扩展
    /// </summary>
    public static class UIExtension
    {
        /// <summary>
        /// 渐变到透明
        /// </summary>
        /// <param name="canvasGroup"></param>
        /// <param name="alpha"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
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
    }

    /// <summary>
    /// Bezier扩展
    /// </summary>
    public static class BezierExtend
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
    /// DateTime扩展
    /// </summary>
    public static class DateTimeExtend
    {
        /// <summary>
        /// 转换日期
        /// </summary>
        public static string ToDateFormat(this DateTime time , string va0 = "年" , string va1 = "月" , string va2 = "日")
        {
            return time.ToString($"yyyy{va0}MM{va1}dd{va2}");
        }

        /// <summary>
        /// 转换时间
        /// </summary>
        public static string ToTimeFormat(this DateTime time , string va0 = "时" , string va1 = "分" , string va2 = "秒")
        {
            return time.ToString($"HH{va0}mm{va1}ss{va2}");
        }

        /// <summary>
        /// 转换日期
        /// </summary>
        public static string ToDateFormats(this DateTime time , string va0 = "年" , string va1 = "月" , string va2 = "日" , string va3 = " " , string va4 = "时" , string va5 = "分" , string va6 = "秒")
        {
            return time.ToString($"yyyy{va0}MM{va1}dd{va2}{va3}HH{va4}mm{va5}ss{va6}");
        }

        /// <summary>
        /// 获取日期是一年中第几个星期
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int WeekOfYear(this DateTime time)
        {
            CultureInfo _cultureInfo = CultureInfo.CurrentCulture;
            return _cultureInfo.Calendar.GetWeekOfYear(time , CalendarWeekRule.FirstDay , DayOfWeek.Monday);
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
        /// 获取时间戳
        /// </summary>
        /// <returns>时间戳</returns>
        public static long GetTimeStamp(bool isMillis = false)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970 , 1 , 1 , 0 , 0 , 0 , 0);
            return isMillis ? Convert.ToInt64(ts.TotalMilliseconds) : Convert.ToInt64(ts.TotalSeconds);
        }
    }

    /// <summary>
    /// Graphic扩展
    /// </summary>
    public static class GraphicExtend
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
    /// list扩展
    /// </summary>
    public static class ListExtend
    {
        /// <summary>
        /// 随机获取一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T RandomListElement<T>(this List<T> list)
        {
            if(list.Count == 0)
            {
                return default;
            }
            if(list.Count == 1)
            {
                return list[0];
            }
            return list[new System.Random( ).Next(0 , list.Count)];
        }

        /// <summary>
        /// 在末尾追加另一list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="bList"></param>
        /// <returns></returns>
        public static List<T> AppendList<T>(this List<T> list , List<T> bList)
        {
            for(int i = 0; i < bList.Count; i++)
            {
                list.Add(bList[i]);
            }
            return list;
        }

        /// <summary>
        /// 把List的全部Item复制到listB中,让两个list所有元素相同,而不是引用关系
        /// </summary>
        public static List<T> CopyTo<T>(this List<T> list , ref List<T> listB)
        {
            listB.Clear( );
            for(int i = 0; i < list.Count; i++)
            {
                listB.Add(list[i]);
            }
            return list;
        }

        /// <summary>
        /// 删除后重复的数据,重复的值只保留一份
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<T> DelRepeat<T>(this List<T> list)
        {
            for(int i = 0; i < list.Count; i++)
            {
                for(int j = list.Count - 1; j > i; j--) //j>i 的意思是:>i前面的已经比较过了
                {
                    var A = list[i].ToString( );
                    var B = list[j].ToString( );
                    if(A == B)
                    {
                        list.RemoveAt(j);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 得到后重复的数据的下标,重复的值只保留一份,
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<int> GetRepeatIndex<T>(this List<T> list)
        {
            var indexList = new List<int>( );
            for(int i = 0; i < list.Count - 1; i++)
            {
                for(int j = list.Count - 1; j > i; j--)
                {
                    var A = list[i].ToString( );
                    var B = list[j].ToString( );
                    if(A == B && !indexList.Contains(j))
                    {
                        indexList.Add(j);
                    }
                }
            }
            return indexList;
        }

        /// <summary>
        /// 从大到小排序
        /// </summary>
        public static List<int> SortDescending<T>(this List<int> list)
        {
            list.Sort((x , y) => -x.CompareTo(y));
            return list;
        }
        /// <summary>
        /// 从大到小排序
        /// </summary>
        public static List<string> SortDescending<T>(this List<string> list)
        {
            list.Sort((x , y) => -x.CompareTo(y));
            return list;
        }

        public static List<T> RandomListFromBigList<T>(this List<T> pList , int randomNum)
        {
            int num = randomNum >= pList.Count ? pList.Count - 1 : randomNum;
            System.Random random = new System.Random(unchecked((int)DateTime.Now.Ticks));
            List<T> list = new List<T>(num);
            for(int i = 0; i < num; i++)
            {
                int r = random.Next(1 , pList.Count);
                T temp = pList[r];
                if(list.Contains(temp))
                {
                    --i;
                    continue;
                }
                else
                {
                    list.Add(temp);
                }
            }
            return list;
        }

        /// <summary>
        /// 列表同步方法封装 - 列表拷贝
        /// </summary>
        public static List<T> ListClone<T>(this List<T> ab)
        {
            List<T> ret = new List<T>( );
            ab.ForEach(item => ret.Add(item));
            return ret;
        }

        public static void ListSafeForEach<T>(this List<T> ab , Func<T , bool> ac)
        {
            List<T> recy = ab.ListClone<T>( );
            foreach(T item in recy)
            {
                if(!ac(item))
                {
                    break;
                }
            }
            recy.Clear( );
        }

        /// <summary>
        /// 列表同步方法封装 - 
        /// </summary>
        /// <param name="item">添加项</param>
        public static void ListAdd<T>(this List<T> ab , T item)
        {
            if(item != null)
            {
                lock(ab)
                {
                    ab.Add(item);
                }
            }
        }
        /// <summary>
        /// 列表同步方法封装 - 添加项, 但不会重复添加同一个对象
        /// </summary>
        /// <param name="item">添加项</param>
        public static void ListAddNotForSelf<T>(this List<T> ab , T item)
        {
            if(item != null)
            {
                lock(ab)
                {
                    if(!ab.Contains(item))
                    {
                        ab.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// 列表同步方法封装
        /// </summary>
        /// <param name="item">删除项</param>
        public static void ListRemove<T>(this List<T> ab , T item)
        {
            if(item != null)
            {
                lock(ab)
                {
                    ab.Remove(item);
                }
            }
        }

        /// <summary>
        /// 清空list
        /// </summary>
        public static void ListClear<T>(this List<T> ab)
        {
            lock(ab)
            {
                ab.Clear( );
            }
        }

        /// <summary>
        /// 获取list长度
        /// </summary>
        public static int ListCount<T>(this List<T> ab)
        {
            lock(ab)
            {
                return ab.Count;
            }
        }
    }

    /// <summary>
    /// LineRenderer扩展
    /// </summary>
    public static class LineRendererExtend
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
    public static class MathExtend
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
        /// 获取随机元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="self">数组</param>
        /// <returns>随机值</returns>
        public static T GetRandomValue<T>(this T[] self)
        {
            return self[Random.Range(0 , self.Length)];
        }

        /// <summary>
        /// 获取随机元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="self">列表</param>
        /// <returns>随机值</returns>
        public static T GetRandomValue<T>(this List<T> self)
        {
            return self[Random.Range(0 , self.Count)];
        }

        /// <summary>
        /// 获取指定个数随机元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="self">数组</param>
        /// <param name="count">个数</param>
        /// <returns>元素数组</returns>
        public static T[] GetRandomValue<T>(this T[] self , int count)
        {
            if(count > self.Length)
            {
                throw new ArgumentOutOfRangeException( );
            }
            List<T> tempList = new List<T>(self.Length);
            for(int i = 0; i < self.Length; i++)
            {
                tempList.Add(self[i]);
            }
            T[] retArray = new T[count];
            for(int i = 0; i < retArray.Length; i++)
            {
                int index = Random.Range(0 , tempList.Count);
                retArray[i] = tempList[index];
                tempList.RemoveAt(index);
            }
            return retArray;
        }

        /// <summary>
        /// 获取指定个数随机元素
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="self">列表</param>
        /// <param name="count">个数</param>
        /// <returns>元素数组</returns>
        public static T[] GetRandomValue<T>(this List<T> self , int count)
        {
            if(count > self.Count)
            {
                throw new ArgumentOutOfRangeException( );
            }
            List<T> tempList = new List<T>(self.Count);
            for(int i = 0; i < self.Count; i++)
            {
                tempList.Add(self[i]);
            }
            T[] retArray = new T[count];
            for(int i = 0; i < retArray.Length; i++)
            {
                int index = Random.Range(0 , tempList.Count);
                retArray[i] = tempList[index];
                tempList.RemoveAt(index);
            }
            return retArray;
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
            float retV = self[0].z * ( self[self.Length - 1].x - self[1].x );
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
            float retV = self[0].z * ( self[self.Count - 1].x - self[1].x );
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
    /// Ray扩展
    /// </summary>
    public static class RayExtend
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
    /// texture扩展
    /// </summary>
    public static class TextureExtend
    {
        /// <summary>
        /// Texture2D转sprite
        /// </summary>
        /// <returns>sprite</returns>
        public static Sprite Texture2DToSprite(this Texture2D texture)
        {
            return Sprite.Create(texture , new Rect(0 , 0 , texture.width , texture.height) , Vector2.zero);
        }
    }

    /// <summary>
    ///  扩展
    /// </summary>
    public static class UnityEngineExtend
    {
        /// <summary>
        /// 设置显示隐藏
        /// </summary>
        /// <param name="component"></param>
        /// <param name="active"></param>
        public static void SetActive(this Component component , bool active)
        {
            if(component != null)
            {
                if(component.gameObject.activeSelf != active)
                {
                    component.gameObject.SetActive(active);
                }
            }
        }

        /// <summary>
        /// 设置物体的本地坐标X
        /// </summary>
        /// <param name="x"></param>
        public static void SetLocalPostionX(this Component component , float x)
        {
            if(component != null)
            {
                Vector3 temp = component.transform.localPosition;
                temp.x = x;
                component.transform.localPosition = temp;
            }
        }

        /// <summary>
        /// 设置物体的本地坐标Y
        /// </summary>
        /// <param name="y"></param>
        public static void SetLocalPostionY(this Component component , float y)
        {
            if(component != null)
            {
                Vector3 temp = component.transform.localPosition;
                temp.y = y;
                component.transform.localPosition = temp;
            }
        }

        /// <summary>
        /// 设置物体的本地坐标Z
        /// </summary>
        /// <param name="z"></param>
        public static void SetLocalPostionZ(this Component component , float z)
        {
            if(component != null)
            {
                Vector3 temp = component.transform.localPosition;
                temp.z = z;
                component.transform.localPosition = temp;
            }
        }
        /// <summary>
        /// 创建物体设置Transform信息
        /// </summary>
        /// <param name="post">位置</param>
        /// <param name="quaternion">旋转</param>
        /// <param name="scale">缩放</param>
        /// <param name="name">物体名</param>
        /// <param name="localPost">采用本地坐标还是世界坐标</param>
        /// <param name="parent">父物体如果为空则不设置</param>
        /// <param name="components">需要添加的组件</param>
        /// <returns></returns>
        public static GameObject CreationObjectSetInfo(Vector3 post , Quaternion quaternion , Vector3 scale , string name = "GameObject" , bool localPost = true , Transform parent = null , params Type[] components)
        {
            GameObject temp = new GameObject(name , components);
            if(parent != null)
            {
                temp.transform.SetParent(parent , false);
            }
            if(localPost)
            {
                temp.transform.SetLocalPositionAndRotation(post , quaternion);
            }
            else
            {
                temp.transform.SetPositionAndRotation(post , quaternion);
            }
            temp.transform.localScale = scale;
            return temp;
        }

        /// <summary>
        /// 设置layer
        /// </summary>
        /// <param name="target"></param>
        /// <param name="layer"></param>
        public static void SetLayer(this GameObject target , int layer)
        {
            if(target.layer != layer)
            {
                target.layer = layer;
                foreach(Transform item in target.transform)
                {
                    if(item.childCount > 0)
                    {
                        SetLayer(item.gameObject , layer);
                    }
                    item.gameObject.layer = layer;
                }
            }
        }

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
                UnityEngine.Debug.LogError(e.Message);
            }
            return null;
        }
    }

    /// <summary>
    ///扩展
    /// </summary>
    public static class ValueExtend
    {

        /// <summary>
        /// string 转 int
        /// </summary>
        /// <param name="str"></param>
        /// <returns>int</returns>
        public static int ToInt(this string str)
        {
            return int.Parse(str);
        }

        ///<summary>
        ///字符串值是有效的Email
        ///</summary>
        public static bool IsValidEmail(this string str)
        {
            return Regex.IsMatch(str , @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        ///<summary>
        ///字符串是否是有效手机号
        ///</summary>
        public static bool IsValidMobilePhoneNumber(this string str)
        {
            return Regex.IsMatch(str , @"^0{0,1}(13[4-9]|15[7-9]|15[0-2]|18[7-8])[0-9]{8}$");
        }

        ///<summary>
        ///字符串内是否包含中文
        ///</summary>
        public static bool ContainCinese(this string str)
        {
            return Regex.IsMatch(str , @"[\u4e00-\u9fa5]");
        }

        /// <summary>
        /// 字符串内是否包含字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ContainLetter(this string str)
        {
            return Regex.Matches(str , "[a-zA-Z]").Count > 0;
        }

        /// <summary>
        /// 字符串值是否为空/空格
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        ///<summary>
        ///字符串是否为空
        ///</summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        ///<summary>
        ///大写第一个字符
        ///</summary>
        public static string UppercaseFirst(this string str)
        {
            return char.ToUpper(str[0]) + str[1..];
        }

        ///<summary>
        ///小写第一个字符
        ///</summary>
        public static string LowercaseFirst(this string str)
        {
            return char.ToLower(str[0]) + str[1..];
        }

        /// <summary>
        /// 字符补齐
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="count">总长度</param>
        /// <param name="polishing">补齐的值</param>
        /// <returns></returns>
        public static string PadLeft(this int value , int count , char polishing = '0')
        {
            return value.ToString( ).PadLeft(count , polishing);
        }

        ///<summary>
        ///使用md5加密字符
        ///</summary>
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

        /// <summary>
        /// 打开windows应用程序
        /// </summary>
        /// <param name="exePath">应用的绝对路径</param>
        public static void OpenApplicationPprogram(this string exePath)
        {
            try
            {
                Process.Start(exePath);
            }
            catch(Exception e)
            {
                UnityEngine.Debug.LogError(e.Message);
            }
        }

    }

    /// <summary>
    /// Vector扩展
    /// </summary>
    public static class VectorExtend
    {
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

    }

    public static class BinaryReaderExtension
    {
        public static Color32 ReadColor32(this BinaryReader binaryReader)
        {
            return new Color32(binaryReader.ReadByte( ) , binaryReader.ReadByte( ) , binaryReader.ReadByte( ) , binaryReader.ReadByte( ));
        }

        public static Color ReadColor(this BinaryReader binaryReader)
        {
            return new Color(binaryReader.ReadSingle( ) , binaryReader.ReadSingle( ) , binaryReader.ReadSingle( ) , binaryReader.ReadSingle( ));
        }

        public static DateTime ReadDateTime(this BinaryReader binaryReader)
        {
            return new DateTime(binaryReader.ReadInt64( ));
        }

        public static Quaternion ReadQuaternion(this BinaryReader binaryReader)
        {
            return new Quaternion(binaryReader.ReadSingle( ) , binaryReader.ReadSingle( ) , binaryReader.ReadSingle( ) , binaryReader.ReadSingle( ));
        }

        public static Rect ReadRect(this BinaryReader binaryReader)
        {
            return new Rect(binaryReader.ReadSingle( ) , binaryReader.ReadSingle( ) , binaryReader.ReadSingle( ) , binaryReader.ReadSingle( ));
        }

        public static Vector2 ReadVector2(this BinaryReader binaryReader)
        {
            return new Vector2(binaryReader.ReadSingle( ) , binaryReader.ReadSingle( ));
        }

        public static Vector3 ReadVector3(this BinaryReader binaryReader)
        {
            return new Vector3(binaryReader.ReadSingle( ) , binaryReader.ReadSingle( ) , binaryReader.ReadSingle( ));
        }

        public static Vector4 ReadVector4(this BinaryReader binaryReader)
        {
            return new Vector4(binaryReader.ReadSingle( ) , binaryReader.ReadSingle( ) , binaryReader.ReadSingle( ) , binaryReader.ReadSingle( ));
        }
    }
}
