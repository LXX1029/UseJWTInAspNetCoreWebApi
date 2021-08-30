using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace JWTClient
{
    /*
     *  创建人：admin
     *  创建日期：2020-1-1
     *  描述：用户模型类
     */
    /// <summary>
    /// 用户模型类
    /// </summary>
    public class UserModel
    {


        private readonly bool Loop = false;
        
        //## 推荐
        /// <summary>
        /// 验证数字委托
        /// </summary>
        public Action<int> NumberValidateAction;

        //## 推荐
        /// <summary>
        /// 验证数字委托
        /// </summary>
        public Func<int,bool> NumberValidateFunc;


        #region 属性
        /// <summary>
        /// 循环次数
        /// </summary>
        public int loop { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public TimeSpan beginTime { get; set; }
        #endregion

        // 推荐
        private int _score = 100;
        // 不推荐
        private int _s = 100;
        /// <summary>
        /// 最大分值
        /// </summary>
        private int _maxScore = 100;
        // 不推荐
        private int _maxscore = 100;

        // 推荐
        private const string SERVER_ERROR = "服务器异常";
        // 不推荐
        private const string serverError = "服务器异常";

        // 不推荐
        public int myProperty { get; set; }
        // 推荐
        private int myVar;
        public int MyProperty
        {
            get { return myVar; }
            set { myVar = value; }
        }
        // 不推荐
        public IEnumerable<Product> GetProducts(string param1, string param2, string param3, string param4)
        {

            return Enumerable.Empty<Product>();
        }
        // 推荐
        public IEnumerable<Product> GetProducts(Tuple<string, string, string, string> param)
        {
            var  productList = Enumerable.Empty<Product>();
            try
            {
              // 从数据库获取Product
            }
            catch (Exception ex)
            {
                // TODO 写入日志文件
                throw ex;
            }
            return productList;
        }


        /// <summary>
        /// 获取所有产品信息
        /// </summary>
        /// <returns>IEnumerable<Product></returns>
        public IEnumerable<Product> GetProducts()
        {
            var productList = Enumerable.Empty<Product>();
            try
            {
                // TODO 从数据库获取Product
            }
            catch (Exception ex)
            {
                // TODO 写入日志文件
                throw ex;
            }
            return productList;
        }

        public void GetUserList()
        {
            // 推荐
            bool isExists = false;
            // 不推荐
            bool a = true;

            // 推荐
 
            // 推荐
            for (int i = 5; i < 100; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    // TODO
                }
            }
            // 不推荐
            string number1 = "number1";
        }
    }
    // 推荐
    public interface IRepository { }
    // 不推荐
    public interface Irepository { }
    // 推荐
    public enum LogEnum { }

    public class Product
    {
       
    }

    public class UserManger
    {
        #region 用户操作
        /// <summary>
        /// 获取用户
        /// </summary>
        public void SetSomething()
        {
            var count = 100; // 这是单行注释
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns>bool true:成功 false:失败</returns>
        public bool DeleteUser(int id) => true;
        #endregion
    }

    //  推荐
    public abstract class ModelBase
    { }




}
public static class Extension
{
    // 推荐
    public static void ForEachExtension<T>(this IEnumerable<T> collection) { }
}

public class ExceptionTest
{
    static double SafeDivision(double x, double y)
    {
        if (y == 0)
            throw new DivideByZeroException();
        return x / y;
    }


   
}
