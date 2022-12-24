namespace Entrogic.Helpers.ID
{
    public static class RarityLevelID
    {
        /// <summary>
        /// 灰色 - Gray
        /// 最低级别。只有钓鱼得到的“垃圾”物品（锡罐、旧鞋和海草）拥有该稀有度，以此作为稀有度基准。
        /// 出现在这个级别的主要是带有糟糕修饰语的白色或蓝色级装备，比如，可耻 铁阔剑。
        /// </summary>
        public const int Waste = ItemRarityID.Gray;
        /// <summary>
        /// 白色 - White
        /// 在泰拉瑞亚的游戏代码中，没有明确指定稀有度的装备都默认归入这一级别。到目前为止，此稀有度的物品是最多的，其中很多都可认为是游戏中最常见的物品。
        /// 其中包含有大多数的家具，建筑材料（物块与墙），早期用低级别的矿石与木材制造出的工具、武器和盔甲，凝胶和草药之类常见制作材料，油漆，以及大多数时装物品。
        /// 此级别的物品会被熔岩摧毁，只有少数例外。
        /// </summary>
        public const int Normal = ItemRarityID.White;
        /// <summary>
        /// 蓝色 - Blue
        /// 早期的矿石制作出的武器与盔甲，以及早期的掉落物品如脚镣与幸运马掌之类。
        /// 也包括旗帜、纪念章、面具，还有困难模式前的各种染料。本级别以及更高级别的物品在熔岩中不会被摧毁。
        /// </summary>
        public const int EarlyPHM = ItemRarityID.Blue;
        /// <summary>
        /// 绿色 - Green
        /// 困难模式前的中期物品。大多数是由掉落或购买（非可制作）得到，
        /// 例外的是死灵盔甲、沙枪、墨镜、黑曜石骷髅头、星星炮、钻石法杖、以及工匠作坊的各种合成品。
        /// </summary>
        public const int MiddlePHM = ItemRarityID.Green;
        /// <summary>
        /// 橙色 - Orange
        /// 困难模式前的后期物品：用狱石制作出来的武器和盔甲、出自丛林和地下丛林的物品、出自地狱物品。
        /// 几种困难模式的矿石、制作材料、和消耗品（强效治疗药水和一些弹药物品）也归于此级别。
        /// </summary>
        public const int LatePHM = ItemRarityID.Orange;
        /// <summary>
        /// 亮红色 - Light Red
        /// 早期困难模式物品，包括由摧毁祭坛之后生出的六种困难模式矿石所制作出来的物品，和困难模式早期或常见敌怪所掉落的物品。
        /// 也包括各种药剂瓶(Flask)，和一些虽然罕见但依然可以在很早期就得到的物品，例如金虫网和史莱姆法杖。
        /// </summary>
        public const int EarlyHM = ItemRarityID.LightRed;
        /// <summary>
        /// 粉色 - Pink
        /// 困难模式中期（世纪之花前）的物品，包括击败机械 Boss得到的那些道具，如火焰喷射器、魔眼法杖和神圣盔甲。
        /// 也包括困难模式 NPC 所出售的那些更昂贵的物品比如环境改造枪，和一些更罕见的困难模式掉落品，其中包括制造中级翅膀所需的一些特殊材料（褴褛蜜蜂之翼、火羽等等）。
        /// </summary>
        public const int MiddleHM = ItemRarityID.Pink;
        /// <summary>
        /// 亮紫色 - LightPurple
        ///这是一个比较小的级别，主要由世纪之花前最罕见的物品所构成，这些物品绝大多数为售卖品或掉落品，
        ///如死神镰刀和钱币枪。也包括高等级的工匠作坊合成品，如十字章护身符。
        /// </summary>
        public const int SpecialMidHM = ItemRarityID.LightPurple;
        /// <summary>
        /// 青柠色 - Lime
        /// 自世纪之花和石巨人，以及困难模式地下丛林的物品。
        /// 叶绿工具与武器、十字章护盾、神庙钥匙、和部分霜月掉落品。
        /// </summary>
        public const int AftMidHM = ItemRarityID.Lime;
        /// <summary>
        /// 黄色 - Yellow
        /// 用世纪之花后的地牢中所获的的物品和以此为材料制作出的物品：蘑菇矿物品，幽灵/灵气物品，生物群落宝箱物品如吸血鬼刀、火星暴乱和南瓜月和霜月事件的掉落品。
        /// 双足翼龙和猪龙鱼公爵的掉落品，蘑菇矿、甲虫、幽灵盔甲套装，幽灵工具套装，泰拉刃，所有坐骑（除了矿车），以及传送枪。
        /// </summary>
        public const int LateHM = ItemRarityID.Yellow;
        /// <summary>
        /// 青色 - Cyan
        ///这又是一个比较小的级别，包含来自月亮事件的早期物品，主要是月亮碎片和天塔柱，以及月亮领主的部分掉落物。
        ///也包括偶尔可以从专家模式的宝藏袋中得到的开发者物品。
        /// </summary>
        public const int EarlyMoon = ItemRarityID.Cyan;
        /// <summary>
        /// 红色 - Red
        ///用月亮碎片和夜明矿在远古操纵机处制作出来的物品。以及一部分直接掉落自月亮领主的物品，包括远古操纵机本身。
        ///还包括某些武器如泰拉刃或配饰如霜花靴的高端重铸版本。
        /// </summary>
        public const int LateMoon = ItemRarityID.Red;
        /// <summary>
        /// 紫色 - Purple
        ///这个级别由拥有高级修饰语的青色与红色物品构成。目前没有基准稀有度在这个级别的物品。
        /// </summary>
        public const int Endgame = ItemRarityID.Purple;
        /// <summary>
        ///这个特殊的级别只包括由专家模式的 Boss 所掉落的宝藏袋中开出的专家模式独有物品，以及宝藏袋本身。
        ///所有来自宝藏袋的专家模式独有物品都拥有此稀有度，除了0x33飞行员、开发者物品和可疑触手。
        ///尽管这些物品在游戏内的稀有度颜色均显示为动态的彩虹色，但在游戏代码中，它们包含有其他等级数值。
        /// </summary>
        public const int Expert = ItemRarityID.Expert;
        /// <summary>
        ///这个特殊的级别包括大师模式中的 Boss 掉落的大师模式独有物品。
        /// </summary>
        public const int Master = ItemRarityID.Master;
        /// <summary>
        /// 这个特殊级别只包含用来交给发布任务的NPC（目前只有染料商和渔夫）来换奖励的“任务物品”。
        /// 任务物品没有其他用途，不过奇异植物可以卖得20 银币。
        /// </summary>
        public const int Amber = ItemRarityID.Quest;
    }
}
