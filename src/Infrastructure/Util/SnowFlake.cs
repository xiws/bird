namespace Infrastructure.Util;

public class SnowFlake
{
        //起始的时间戳
    private static long START_STMP = 1480166465631L;

    //每一部分占用的位数
    private static int SEQUENCE_BIT = 12; //序列号占用的位数
    private static int MACHINE_BIT = 5;   //机器标识占用的位数
    private static int DATACENTER_BIT = 5;//数据中心占用的位数

    //每一部分的最大值
    private static long MAX_DATACENTER_NUM = -1L ^ (-1L << DATACENTER_BIT);
    private static long MAX_MACHINE_NUM = -1L ^ (-1L << MACHINE_BIT);
    private static long MAX_SEQUENCE = -1L ^ (-1L << SEQUENCE_BIT);

    //每一部分向左的位移
    private static int MACHINE_LEFT = SEQUENCE_BIT;
    private static int DATACENTER_LEFT = SEQUENCE_BIT + MACHINE_BIT;
    private static int TIMESTMP_LEFT = DATACENTER_LEFT + DATACENTER_BIT;

    private long datacenterId = 1;  //数据中心
    private long machineId = 1;     //机器标识
    private long sequence = 0L; //序列号
    private long lastStmp = -1L;//上一次时间戳

    #region 单例:完全懒汉
    private static readonly Lazy<SnowFlake> lazy = new Lazy<SnowFlake>(() => new SnowFlake());
    public static SnowFlake Singleton { get { return lazy.Value; } }
    
    private SnowFlake() { }
    #endregion

    public SnowFlake(long datacenterId, long machineId)
    {
        if (datacenterId > MAX_DATACENTER_NUM || datacenterId < 0) throw new Exception($"中心Id应在(0,{MAX_DATACENTER_NUM})之间");
        if (machineId > MAX_MACHINE_NUM || machineId < 0) throw new Exception($"机器Id应在(0,{MAX_MACHINE_NUM})之间");
        this.datacenterId = datacenterId;
        this.machineId = machineId;
    }

    /// <summary>
    /// 产生下一个ID
    /// </summary>
    /// <returns></returns>
    public long NextId()
    {
        long currStmp = GetNewsTmp();
        if (currStmp < lastStmp) throw new Exception("时钟倒退，Id生成失败！");

        if (currStmp == lastStmp)
        {
            //相同毫秒内，序列号自增
            sequence = (sequence + 1) & MAX_SEQUENCE;
            //同一毫秒的序列数已经达到最大
            if (sequence == 0L) currStmp = GetNextMill();
        }
        else
        {
            //不同毫秒内，序列号置为0
            sequence = 0L;
        }

        lastStmp = currStmp;

        return (currStmp - START_STMP) << TIMESTMP_LEFT       //时间戳部分
                      | datacenterId << DATACENTER_LEFT       //数据中心部分
                      | machineId << MACHINE_LEFT             //机器标识部分
                      | sequence;                             //序列号部分
    }

    private long GetNextMill()
    {
        long mill = GetNewsTmp();
        while (mill <= lastStmp)
        {
            mill = GetNewsTmp();
        }
        return mill;
    }

    private long GetNewsTmp()
    {
        return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
    }
}