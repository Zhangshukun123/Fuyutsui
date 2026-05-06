local _, fu = ...
if fu.classId ~= 11 then return end

Fuyutsui.ClassBlocks = {
    [1] = {
        [1] = { type = "block", name = "锚点" },
        [2] = { type = "block", name = "职业" },
        [3] = { type = "block", name = "专精" },
        [4] = { type = "block", name = "有效性" },
        [5] = { type = "block", name = "战斗" },
        [6] = { type = "block", name = "移动" },
        [7] = { type = "block", name = "施法" },
        [8] = { type = "block", name = "引导" },
        [9] = { type = "block", name = "蓄力" },
        [10] = { type = "block", name = "蓄力层数" },
        [11] = { type = "block", name = "生命值" },
        [12] = { type = "block", name = "能量值" },
        [13] = { type = "block", name = "一键辅助" },
        [14] = { type = "block", name = "法术失败" },
        [15] = { type = "block", name = "目标类型" },
        [16] = { type = "block", name = "队伍类型" },
        [17] = { type = "block", name = "队伍人数" },
        [18] = { type = "block", name = "首领战" },
        [19] = { type = "block", name = "难度" },
        [20] = { type = "block", name = "英雄天赋" },

        [21] = { type = "block", name = "目标生命值" },
        [22] = { type = "block", name = "敌人人数" },
        [23] = { type = "block", name = "姿态" },

        [31] = { type = "spell", spellId = 22812, name = "树皮术" },
        [32] = { type = "spell", spellId = 132469, name = "台风" },
        [33] = { type = "spell", spellId = 99, name = "夺魂咆哮" },
        [34] = { type = "spell", spellId = 29166, name = "激活" },
        [35] = { type = "spell", spellId = 102793, name = "乌索尔旋风" },
        [36] = { type = "spell", spellId = 78675, name = "日光术" },

    },
    [2] = {
        [1] = { type = "block", name = "锚点" },
        [2] = { type = "block", name = "职业" },
        [3] = { type = "block", name = "专精" },
        [4] = { type = "block", name = "有效性" },
        [5] = { type = "block", name = "战斗" },
        [6] = { type = "block", name = "移动" },
        [7] = { type = "block", name = "施法" },
        [8] = { type = "block", name = "引导" },
        [9] = { type = "block", name = "蓄力" },
        [10] = { type = "block", name = "蓄力层数" },
        [11] = { type = "block", name = "生命值" },
        [12] = { type = "block", name = "能量值" },
        [13] = { type = "block", name = "一键辅助" },
        [14] = { type = "block", name = "法术失败" },
        [15] = { type = "block", name = "目标类型" },
        [16] = { type = "block", name = "队伍类型" },
        [17] = { type = "block", name = "队伍人数" },
        [18] = { type = "block", name = "首领战" },
        [19] = { type = "block", name = "难度" },
        [20] = { type = "block", name = "英雄天赋" },

        [21] = { type = "block", name = "目标生命值" },
        [22] = { type = "block", name = "敌人人数" },
        [23] = { type = "block", name = "姿态" },

        [31] = { type = "spell", spellId = 22812, name = "树皮术" },
        [32] = { type = "spell", spellId = 132469, name = "台风" },
        [33] = { type = "spell", spellId = 99, name = "夺魂咆哮" },
        [34] = { type = "spell", spellId = 29166, name = "激活" },
        [35] = { type = "spell", spellId = 102793, name = "乌索尔旋风" },
    },
    [3] = {
        [1] = { type = "block", name = "锚点" },
        [2] = { type = "block", name = "职业" },
        [3] = { type = "block", name = "专精" },
        [4] = { type = "block", name = "有效性" },
        [5] = { type = "block", name = "战斗" },
        [6] = { type = "block", name = "移动" },
        [7] = { type = "block", name = "施法" },
        [8] = { type = "block", name = "引导" },
        [9] = { type = "block", name = "蓄力" },
        [10] = { type = "block", name = "蓄力层数" },
        [11] = { type = "block", name = "生命值" },
        [12] = { type = "block", name = "能量值" },
        [13] = { type = "block", name = "一键辅助" },
        [14] = { type = "block", name = "法术失败" },
        [15] = { type = "block", name = "目标类型" },
        [16] = { type = "block", name = "队伍类型" },
        [17] = { type = "block", name = "队伍人数" },
        [18] = { type = "block", name = "首领战" },
        [19] = { type = "block", name = "难度" },
        [20] = { type = "block", name = "英雄天赋" },

        [21] = { type = "block", name = "目标生命值" },
        [22] = { type = "block", name = "敌人人数" },
        [23] = { type = "block", name = "姿态" },

        [24] = { type = "aura", name = "塞纳留斯的梦境", auraName = "塞纳留斯的梦境", showKey = "remaining" },
        [25] = { type = "aura", name = "塞纳留斯的梦境层数", auraName = "塞纳留斯的梦境", showKey = "count" },
        [26] = { type = "aura", name = "铁鬃", auraName = "铁鬃", showKey = "remaining" },
        [27] = { type = "aura", name = "狂暴回复", auraName = "狂暴回复", showKey = "remaining" },

        [31] = { type = "spell", spellId = 22812, name = "树皮术" },
        [32] = { type = "spell", spellId = 132469, name = "台风" },
        [33] = { type = "spell", spellId = 99, name = "夺魂咆哮" },
        [34] = { type = "spell", spellId = 29166, name = "激活" },
        [35] = { type = "spell", spellId = 102793, name = "乌索尔旋风" },

        [36] = { type = "spell", spellId = 22842, name = "狂暴回复" },
        [37] = { type = "spell", spellId = 22842, name = "狂暴回复", charge = true },
        [38] = { type = "spell", spellId = 61336, name = "生存本能" },
        [39] = { type = "spell", spellId = 102558, name = "化身：乌索克的守护者" },
        [40] = { type = "spell", spellId = 1261867, name = "野性之心" },

    },
    [4] = {
        [1] = { type = "block", name = "锚点" },
        [2] = { type = "block", name = "职业" },
        [3] = { type = "block", name = "专精" },
        [4] = { type = "block", name = "有效性" },
        [5] = { type = "block", name = "战斗" },
        [6] = { type = "block", name = "移动" },
        [7] = { type = "block", name = "施法" },
        [8] = { type = "block", name = "引导" },
        [9] = { type = "block", name = "蓄力" },
        [10] = { type = "block", name = "蓄力层数" },
        [11] = { type = "block", name = "生命值" },
        [12] = { type = "block", name = "能量值" },
        [13] = { type = "block", name = "一键辅助" },
        [14] = { type = "block", name = "法术失败" },
        [15] = { type = "block", name = "目标类型" },
        [16] = { type = "block", name = "队伍类型" },
        [17] = { type = "block", name = "队伍人数" },
        [18] = { type = "block", name = "首领战" },
        [19] = { type = "block", name = "难度" },
        [20] = { type = "block", name = "英雄天赋" },

        [21] = { type = "block", name = "姿态" },
        [22] = { type = "block", name = "目标距离" },
        [23] = { type = "block", name = "连击点" },
        [24] = { type = "block", name = "施法技能" },

        [29] = { type = "aura", name = "节能施法", auraName = "节能施法", showKey = "remaining" },
        [30] = { type = "aura", name = "丛林之魂", auraName = "丛林之魂", showKey = "remaining" },

        [31] = { type = "spell", spellId = 22812, name = "树皮术" },
        [32] = { type = "spell", spellId = 132469, name = "台风" },
        [33] = { type = "spell", spellId = 99, name = "夺魂咆哮" },
        [34] = { type = "spell", spellId = 29166, name = "激活" },
        [35] = { type = "spell", spellId = 102793, name = "乌索尔旋风" },

        [36] = { type = "spell", spellId = 18562, name = "迅捷治愈" },
        [37] = { type = "spell", spellId = 18562, name = "迅捷治愈", charge = true },
        [38] = { type = "spell", spellId = 48438, name = "野性成长" },
        [39] = { type = "spell", spellId = 391528, name = "万灵之召" },
        [40] = { type = "spell", spellId = 88423, name = "自然之愈" },
        [41] = { type = "spell", spellId = 102342, name = "铁木树皮" },
        [42] = { type = "spell", spellId = 132158, name = "自然迅捷" },
        [43] = { type = "spell", spellId = 1261867, name = "野性之心" },

        [45] = {
            type = "group",
            num = 7,
            healthPercent = 1,
            role = 2,
            dispel = 3,
            aura = {
                [4] = { 33763 },                    -- 生命绽放
                [5] = { 48438, 8936, 774, 155777 }, -- 迅捷治愈(回春术, 萌芽, 愈合, 野性生长)
                [6] = { 8936 },                     -- 愈合
            },
            rejuv = 7,
        },
    },
}
Fuyutsui.MacrosList = {
    dynamicSpells = { "回春术", "愈合", "生命绽放", "迅捷治愈", "自然之愈" },
    specialSpells = { [17] = "/cancelaura [spec:4]猎豹形态\n/cast 万灵之召", },
    staticSpells = {
        [1]  = "[nostance:2]猎豹形态(变形)",
        [2]  = "[nostance:1]熊形态(变形)",
        [3]  = "[nostance:4]枭兽形态",
        [4]  = "月火术",
        [5]  = "树皮术",
        [6]  = "横扫",
        [7]  = "潜行",
        [8]  = "凶猛撕咬",
        [9]  = "愤怒",
        [10] = "割裂",
        [11] = "撕碎",
        [12] = "斜掠",
        [13] = "痛击",
        [14] = "野性印记",
        [15] = "裂伤",
        [16] = "野性成长",
        [18] = "自然迅捷",
        [19] = "[@player]激活",
        [20] = "野性之心",
        [21] = "野性冲锋",
        [22] = "铁鬃",
        [23] = "摧折",
        [24] = "明月普照",
        [25] = "狂暴回复",
        [26] = "台风",
        [27] = "夺魂咆哮",
        [28] = "[@cursor]乌索尔旋风",
        [29] = "日光术",
        [30] = "星涌术",
        [31] = "星火术",
        [32] = "星辰坠落",
        [33] = "自然之力",
        [34] = "日蚀",
        [35] = "超凡之盟",
        [36] = "化身：艾露恩之眷",
        [37] = "艾露恩之怒",
        [38] = "野性蘑菇",
        [39] = "新月",
        [40] = "阳炎术",
        [41] = "月蚀",
        [42] = "化身：阿莎曼之灵",
        [43] = "原始之怒",
        [44] = "迎头痛击",
        [45] = "怒意狂乱",
        [46] = "猛虎之怒",
        [47] = "生存本能",
        [48] = "野性冲锋",
        [49] = "群体缠绕",
        [50] = "狂暴",
        [51] = "啃噬",
        [52] = "野性狂乱",
    },
}


function fu.updateSpecInfo()
    local specIndex = C_SpecializationInfo.GetSpecialization()
    fu.powerType = nil
    fu.blocks = nil
    fu.countBars = nil
    fu.group_blocks = nil
    fu.assistant_spells = nil
    if specIndex == 1 then
        fu.blocks = {
            ["目标生命值"] = 21,
            ["敌人人数"] = 22,
            ["姿态"] = 23,
            auras = {

            },
        }
        fu.spellCooldown[78675] = { index = 36, name = "日光术" }
    elseif specIndex == 2 then
        fu.blocks = {
            ["目标生命值"] = 21,
            ["敌人人数"] = 22,
            ["姿态"] = 23,
            auras = {

            },
        }
    elseif specIndex == 3 then
        fu.powerType = "RAGE"
        fu.blocks = {
            ["目标生命值"] = 21,
            ["敌人人数"] = 22,
            ["姿态"] = 23,
            auras = {
                ["塞纳留斯的梦境"] = {
                    index = 24,
                    auraRef = fu.Auras["塞纳留斯的梦境"],
                    showKey = "remaining",
                },
                ["塞纳留斯的梦境层数"] = {
                    index = 25,
                    auraRef = fu.Auras["塞纳留斯的梦境"],
                    showKey = "count",
                },
                ["铁鬃"] = {
                    index = 26,
                    auraRef = fu.Auras["铁鬃"],
                    showKey = "remaining",
                },
                ["狂暴回复"] = {
                    index = 27,
                    auraRef = fu.Auras["狂暴回复"],
                    showKey = "remaining",
                },
            },
        }
        fu.spellCooldown[22842] = { index = 36, name = "狂暴回复", charge = 37 }
        fu.spellCooldown[61336] = { index = 38, name = "生存本能" }
        fu.spellCooldown[102558] = { index = 39, name = "化身：乌索克的守护者" }
        fu.spellCooldown[1261867] = { index = 40, name = "野性之心" }
    elseif specIndex == 4 then
        fu.powerType = "MANA"
        fu.blocks = {
            ["姿态"] = 21,
            ["目标距离"] = 22,
            ["连击点"] = 23,
            ["施法技能"] = 24,
            auras = {
                ["节能施法"] = {
                    index = 29,
                    auraRef = fu.Auras["节能施法"],
                    showKey = "remaining",
                },
                ["丛林之魂"] = {
                    index = 30,
                    auraRef = fu.Auras["丛林之魂"],
                    showKey = "remaining",
                },
            },

        }

        fu.spellCooldown[18562] = { index = 36, name = "迅捷治愈", charge = 37 }
        fu.spellCooldown[48438] = { index = 38, name = "野性成长" }
        fu.spellCooldown[391528] = { index = 39, name = "万灵之召" }
        fu.spellCooldown[88423] = { index = 40, name = "自然之愈" }
        fu.spellCooldown[102342] = { index = 41, name = "铁木树皮" }
        fu.spellCooldown[132158] = { index = 42, name = "自然迅捷" }
        fu.spellCooldown[1261867] = { index = 43, name = "野性之心" }

        fu.group_blocks = {
            unit_start = 45,
            block_num = 7,
            healthPercent = 1,
            role = 2,
            dispel = 3,
            aura = {
                [4] = { 33763 },                    -- 生命绽放
                [5] = { 48438, 8936, 774, 155777 }, -- 迅捷治愈(回春术, 萌芽, 愈合, 野性生长)
                [6] = { 8936 },                     -- 愈合
            },
            rejuv = 7,                              -- 回春术数量
        }
    end
end

-- 创建德鲁伊宏
function fu.CreateClassMacro()
    local dynamicSpells = { "回春术", "愈合", "生命绽放", "迅捷治愈", "自然之愈" }
    local specialSpells = { [17] = "/cancelaura [spec:4]猎豹形态\n/cast 万灵之召", }
    local staticSpells = {
        [1]  = "[nostance:2]猎豹形态(变形)",
        [2]  = "[nostance:1]熊形态(变形)",
        [3]  = "[nostance:4]枭兽形态",
        [4]  = "月火术",
        [5]  = "树皮术",
        [6]  = "横扫",
        [7]  = "潜行",
        [8]  = "凶猛撕咬",
        [9]  = "愤怒",
        [10] = "割裂",
        [11] = "撕碎",
        [12] = "斜掠",
        [13] = "痛击",
        [14] = "野性印记",
        [15] = "裂伤",
        [16] = "野性成长",
        [18] = "自然迅捷",
        [19] = "[@player]激活",
        [20] = "野性之心",
        [21] = "野性冲锋",
        [22] = "铁鬃",
        [23] = "摧折",
        [24] = "明月普照",
        [25] = "狂暴回复",
        [26] = "台风",
        [27] = "夺魂咆哮",
        [28] = "[@cursor]乌索尔旋风",
        [29] = "日光术",
        [30] = "星涌术",
        [31] = "星火术",
        [32] = "星辰坠落",
        [33] = "自然之力",
        [34] = "日蚀",
        [35] = "超凡之盟",
        [36] = "化身：艾露恩之眷",
        [37] = "艾露恩之怒",
        [38] = "野性蘑菇",
        [39] = "新月",
        [40] = "阳炎术",
        [41] = "月蚀",
        [42] = "化身：阿莎曼之灵",
        [43] = "原始之怒",
        [44] = "迎头痛击",
        [45] = "怒意狂乱",
        [46] = "猛虎之怒",
        [47] = "生存本能",
        [48] = "野性冲锋",
        [49] = "群体缠绕",
        [50] = "狂暴",
        [51] = "啃噬",
        [52] = "野性狂乱",
    }
    fu.CreateMacro(dynamicSpells, staticSpells, specialSpells)
end
