using System.Collections.Generic;
using UnityEngine;

namespace Wrapper
{
    /// <summary>
    /// 디버그 필터
    /// </summary>
    [System.Serializable]
    public class DebugFilter
    {
        /// <summary>
        /// 디버그 필터 플래그
        /// </summary>
        public object pFilterFlag;

        /// <summary>
        /// 디버그 로그의 색상값
        /// </summary>
        public string strColorHexCode;

        /// <summary>
        /// 디버그 필터당 정보 생성 (보다 디테일한 필터 설정을)
        /// </summary>
        /// <param name="pFilterFlag">디버그 필터 플래그</param>
        public DebugFilter(object pFilterFlag)
        {
            this.pFilterFlag = pFilterFlag;
        }
    }

    /// <summary>
    /// 디버그 필터 생성기
    /// </summary>
    public class DebugFilterFactory
    {
        /// <summary>
        /// 현재 저장된 DebugFilter입니다.
        /// </summary>
        public IEnumerable<DebugFilter> arrDebugFilter => _listDebugFilter;

        List<DebugFilter> _listDebugFilter = new List<DebugFilter>();

        /// <summary>
        /// Debug Filter를 추가합니다.
        /// </summary>
        /// <param name="pFilterFlag">추가할 필터</param>
        public DebugFilter DoAdd_DebugFilter(object pFilterFlag)
        {
            DebugFilter pFilter = new DebugFilter(pFilterFlag);
            _listDebugFilter.Add(pFilter);

            return pFilter;
        }

        /// <summary>
        /// Debug Filter를 추가합니다.
        /// </summary>
        /// <param name="pFilterFlag">추가할 필터</param>
        /// <param name="sColor">출력할 색상</param>
        public DebugFilter DoAdd_DebugFilter(object pFilterFlag, Color sColor)
        {
            DebugFilter pFilter = DoAdd_DebugFilter(pFilterFlag);
            pFilter.strColorHexCode = ColorUtility.ToHtmlStringRGB(sColor);

            return pFilter;
        }
    }
}
