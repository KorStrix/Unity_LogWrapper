using UnityEngine;

namespace CustomDebug
{
    /// <summary>
    /// 
    /// </summary>
    public enum EOperatorType
    {
        None,
        AND,
    }

    /// <summary xml:lang="fr">
    /// Custom <see cref="Debug"/> 로그 출력에 필요한 Interface
    /// </summary>
    public interface ICustomLogType
    {
        /// <summary>
        /// 주석
        /// </summary>
        string Comment { get; }

        /// <summary>
        /// 디버그 필터 플래그
        /// </summary>
        string LogTypeName { get; }

        /// <summary>
        /// 플래그 체크용 ulong 값
        /// </summary>
        ulong Number { get; }

        /// <summary>
        /// 필터의 정보
        /// <para>Ex) 흰색 : ffffff</para>
        /// <para>Ex) 빨간색 : ff0000</para>
        /// </summary>
        string ColorHexCode { get; }
        EOperatorType eOperatorType { get; }
    }

    /// <summary>
    /// 디버그 필터
    /// </summary>
    [System.Serializable]
    public class DefaultLogType : ICustomLogType
    {
        /// <summary>
        /// <see cref="Debug.Log(object)"/>로 출력하고 싶은 경우 이 플래그를 넣으시면 됩니다
        /// </summary>
        public static DefaultLogType Default = new DefaultLogType(nameof(Default), 0, "ffffff");


        public string Comment => strComment;
        public string LogTypeName => strLogTypeName;
        public ulong Number => lNumber;
        public string ColorHexCode => strColorHexCode;
        public EOperatorType eOperatorType => _eOperatorType;


        /// <summary>
        /// 주석
        /// </summary>
        public string strComment;

        /// <summary>
        /// 디버그 필터 플래그
        /// </summary>
        public string strLogTypeName;

        /// <summary>
        /// 플래그 체크용 ulong 값
        /// </summary>
        public ulong lNumber;

        /// <summary>
        /// 필터의 정보
        /// <para>Ex) 흰색 : ffffff</para>
        /// <para>Ex) 빨간색 : ff0000</para>
        /// </summary>
        public string strColorHexCode;

        private EOperatorType _eOperatorType;


        /// <summary>
        /// 필터의 정보
        /// </summary>
        /// <param name="strLogTypeName">디버그 필터 플래그</param>
        /// <param name="lNumber">플래그 체크할 숫자</param>
        /// <param name="strColorHexCode">색상 코드 (Ex. 흰색 : ffffff)</param>
        public DefaultLogType(string strLogTypeName, ulong lNumber, string strColorHexCode = "ffffff")
        {
            this.strLogTypeName = strLogTypeName;
            this.lNumber = lNumber;
            this.strColorHexCode = strColorHexCode;
        }


        #region operator

        public static DefaultLogType operator |(DefaultLogType a, DefaultLogType b)
        {
            DefaultLogType pNewLogType = new DefaultLogType(
                $"({a.strLogTypeName}|{b.strLogTypeName})", a.lNumber | b.lNumber);

            return pNewLogType;
        }

        public static DefaultLogType operator &(DefaultLogType a, DefaultLogType b)
        {
            DefaultLogType pNewLogType = new DefaultLogType(
                $"({a.strLogTypeName}&{b.strLogTypeName})", a.lNumber & b.lNumber);

            return pNewLogType;
        }

        #endregion
    }
}