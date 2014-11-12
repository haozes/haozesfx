using System;
using System.Collections.Generic;
using System.Text;

namespace Haozes.FxClient.Core
{
    /// <summary>
    /// 客户端在线状态
    /// </summary>
    public enum PresenceStatus
    {
        /// <summary>
        /// 隐身
        /// </summary>
        Invisible=0,

        /// <summary>
        /// 离开
        /// </summary>
        Away=100,

        /// <summary>
        /// 上线
        /// </summary>
        Active=400,

        /// <summary>
        /// 离开
        /// </summary>
        Busy=600,
        /// <summary>
        /// 接听电话
        /// </summary>
        InCall=500,

        /// <summary>
        /// 马上回来
        /// </summary>
        BeRightBack=300,

        /// <summary>
        /// 外出就餐
        /// </summary>
        AtLunch=150
    }
}
