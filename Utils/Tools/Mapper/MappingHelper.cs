using AutoMapper;
using System.Collections;
using System.Collections.Generic;

namespace System
{
    public static class MappingHelper
    {
        /// <summary>
        /// 模型转换
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static TResult MapTo<TFrom, TResult>(this TFrom self)
        {
            if (self == null) throw new ArgumentNullException();
            Mapper.Map(typeof(TFrom), typeof(TResult));
            return (TResult)Mapper.Map(self, typeof(TFrom), typeof(TResult));
        }

        /// <summary>
        /// 模型转换（结果对象已事先创建）
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static TResult MapTo<TFrom, TResult>(this TFrom self, TResult result)
        {
            if (self == null) throw new ArgumentNullException();
            Mapper.Map(typeof(TFrom).UnderlyingSystemType, typeof(TResult));
            return (TResult)Mapper.Map(self, result, typeof(TFrom), typeof(TResult));
        }

        /// <summary>
        /// 列表转换
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        public static IList<TResult> MapToList<TFrom, TResult>(this IEnumerable<TFrom> self)
        {
            if (self == null) throw new ArgumentNullException();
            Mapper.Map(typeof(IEnumerable<TFrom>), typeof(IList<TResult>));
            return (IList<TResult>)Mapper.Map(self, typeof(IEnumerable<TFrom>), typeof(IList<TResult>));
        }

        /// <summary>
        /// 列表转换（结果列表已事先创建）
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IList<TResult> MapToList<TFrom, TResult>(this IEnumerable<TFrom> self, IList<TResult> result)
        {
            if (self == null) throw new ArgumentNullException();
            Mapper.Map(typeof(IEnumerable<TFrom>).UnderlyingSystemType, typeof(IList<TResult>));
            return (IList<TResult>)Mapper.Map(self, result, typeof(IEnumerable<TFrom>), typeof(IList<TResult>));
        }

        public static TDestination MapperConvert<TDestination>(this object source)
        {
            return Mapper.Map<TDestination>(source);
        }
    }
}