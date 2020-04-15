namespace GS.Common.Utils
{
    public static class MoneyExtension
    {
        #region 分转元

        /// <summary>
        /// 分转元
        /// </summary>
        /// <param name="points">金额，单位分</param>
        /// <returns></returns>
        public static decimal PointsToYuan(this int points)
        {
            return points / 100.00M;
        }

        #endregion

        #region 元转分

        /// <summary>
        /// 元转分
        /// </summary>
        /// <param name="yuan">金额，单位元</param>
        /// <returns></returns>
        public static int YuanToPoints(this decimal yuan)
        {
            return (int)(yuan * 100);
        }
        #endregion
    }
}
