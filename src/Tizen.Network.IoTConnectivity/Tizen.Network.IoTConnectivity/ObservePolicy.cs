/// Copyright 2016 by Samsung Electronics, Inc.,
///
/// This software is the confidential and proprietary information
/// of Samsung Electronics, Inc. ("Confidential Information"). You
/// shall not disclose such Confidential Information and shall use
/// it only in accordance with the terms of the license agreement
/// you entered into with Samsung.

namespace Tizen.Network.IoTConnectivity
{
    /// <summary>
    /// Enumeration for policy of observation
    /// </summary>
    public enum ObservePolicy
    {
        /// <summary>
        /// Indicates observation request for most up-to-date notifications only
        /// </summary>
        IgnoreOutOfOrder = 0,
        /// <summary>
        /// Indicates observation request for all notifications including stale notifications
        /// </summary>
        AcceptOutOfOrder
    }
}
