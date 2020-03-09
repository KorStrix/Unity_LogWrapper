using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityEngine;
using Wrapper;

namespace DLL_Test
{
    [TestClass]
    public class UnitTest
    {
        [System.Flags]
        enum ETestFlag
        {
            Network_Recv = 1 << 0,
            Network = 1 << 1,
        }

        [TestMethod]
        public void TestMethod1()
        {
            Color sColor_NetworkRecv = new Color(1f, 0f, 0f);
            Color sColor_Network = new Color(0f, 1f, 0f);

            DebugFilterFactory pFactory = new DebugFilterFactory();
            pFactory.DoAdd_DebugFilter(ETestFlag.Network_Recv, sColor_NetworkRecv);
            pFactory.DoAdd_DebugFilter(ETestFlag.Network, sColor_Network);
            Wrapper.Debug.Init_PrintLog_FilterFlag(pFactory);

            string strOutResult;
            string strResult;

            Wrapper.Debug.LogFormat_Default(ETestFlag.Network_Recv | ETestFlag.Network, ETestFlag.Network_Recv | ETestFlag.Network, out strOutResult);
            strResult = $"<b>[<color=#{ColorUtility.ToHtmlStringRGB(sColor_NetworkRecv)}>{ETestFlag.Network_Recv}</color>, <color=#{ColorUtility.ToHtmlStringRGB(sColor_Network)}>{ETestFlag.Network}</color>]</b> {ETestFlag.Network_Recv | ETestFlag.Network}";
            Assert.AreEqual(strOutResult, strResult);

            Wrapper.Debug.LogFormat_Default(ETestFlag.Network_Recv, ETestFlag.Network_Recv, out strOutResult);
            strResult = $"<b>[<color=#{ColorUtility.ToHtmlStringRGB(sColor_NetworkRecv)}>{ETestFlag.Network_Recv}</color>]</b> {ETestFlag.Network_Recv}";
            Assert.AreEqual(strOutResult, strResult);

            Wrapper.Debug.LogFormat_Default(ETestFlag.Network, ETestFlag.Network, out strOutResult);
            strResult = $"<b>[<color=#{ColorUtility.ToHtmlStringRGB(sColor_Network)}>{ETestFlag.Network}</color>]</b> {ETestFlag.Network}";
            Assert.AreEqual(strOutResult, strResult);
        }
    }
}
