using Xunit;

namespace LrcParser.Tests
{
    public class LyricParseTests
    {
        [Fact]
        public void ParseLyric()
        {
            var lyricText = @"
            [00:00.000] 作词 : Kevinz\n
            [00:00.000] 作曲 : Kevinz\n
            [00:00.00]「Say a Good Bye」\n
            [00:05.27]“说/一个/很好的/再见。”\n
            [00:10.63]词曲/Kevinz\n
            [00:21.34]调混/URUUT\n
            [00:32.30]\n
            [00:32.31]迫降海鸥的集市街 吧台窄窄的寿司店\n
            [00:37.63]酱油碟翻滚几个圈 玩笑话里泛着清甜\n
            [00:43.00]红叶满腔沸腾热血 骇得树荫抖落地面\n
            [00:48.28]杂耍艺人的魔术伞 开出深秋的艳阳天\n
            [00:53.63]\n
            [00:53.64]夕阳中列车正离站 驮着晚霞走得很远\n
            [00:58.97]街头巷尾灯光蹒跚 映得影子跌撞打转\n
            [01:04.32]香气直冒的便当店 暖洋播放着中孝介\n
            [01:09.64]单车溜过傍晚公园 惹得落花纷纷纠缠\n
            [01:15.40]\n
            [01:15.41]我也曾 跨越过远山 寻蜷于虹彩之中的溪泉\n
            [01:20.62]也试图 把柔光打散 调月色绘满眼瞳看世界\n
            [01:25.85]竟要感谢 你令我 沉浸于这异国迷路的喜悦\n
            [01:31.16]随处看见 也不吝胶卷 收纳于一匣方寸天地间\n
            [01:36.62]Saya- saya- say a good bye-\n[01:58.63]\n
            [01:58.64]---Music---\n
            [02:20.28]\n
            [02:20.29]窸窸窣窣落下雨点 霓虹灯遮了一层帘\n
            [02:25.62]远处高楼看不真切 反而记忆得更真切\n
            [02:30.94]银杏展成烫金地毯 枫叶流苏优雅垂边\n
            [02:36.28]添了胡桃的温拿铁 凭热气就引人垂涎\n
            [02:41.63]\n
            [02:41.64]弹起吉他的小少年 每个音符都挺腼腆\n
            [02:46.97]夜城开始热闹非凡 藏着无穷尽的新鲜\n
            [02:52.29]走走停停几度时年 我是渺小过客一员\n
            [02:57.62]再穿过这道斑马线 星群仍于城市贪欢\n
            [03:03.40]\n
            [03:03.41]我也曾 跨越过远山 寻蜷于虹彩之中的溪泉\n
            [03:08.63]也试图 把柔光打散 调月色绘满眼瞳看世界\n
            [03:13.83]竟要感谢 你令我 沉浸于这异国迷路的喜悦\n
            [03:19.13]随处看见 也不吝胶卷 收纳于一匣方寸天地间\n
            [03:24.65]Saya- saya- say a good bye-\n
            [03:46.63]\n
            [03:46.64]---Music---\n
            [04:07.32]\n
            [04:07.33]到最后 这一座夜晚 仿佛我恍惚中有些流连\n
            [04:12.65]隔着窗 仅我未入眠 却看清满目温柔与安然\n
            [04:17.81]我仍珍惜 你予我 这一段随时光起舞的情节\n
            [04:23.14]流景绸缎 奔波于面前 不分昼夜呢喃万语千言\n
            [04:28.47]\n
            [04:28.48]有点抱歉 我将要离开 带着浸透了晨光的烟霭\n
            [04:33.78]希望能够 再次相遇时 我成为想要成为的存在\n
            [04:39.13]再次感谢 一时间 我不知是否亏欠这份善待\n
            [04:44.49]光阴瀚海 正纷至沓来 车厢边如此瑰丽的意外\n
            [04:49.98]Saya-saya-say a good bye-\n
            [05:11.98]\n
            [05:11.99]---Fin---\n";

            var lyrics = new LyricParser().Parse(lyricText);
            Assert.Equal(54, lyrics.Count);
        }
    }
}