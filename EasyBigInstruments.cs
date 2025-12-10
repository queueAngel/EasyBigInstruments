using MonoMod.Cil;
using System;
using System.Reflection;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.UI;
using ThoriumMod.UI.ResourceBars;

namespace EasyBigInstruments;

public class EasyBigInstruments : Mod
{
    public EasyBigInstruments()
    {
        var timerLoad = typeof(ThoriumInterfaceResources).GetMethod(nameof(Load));
        MonoModHooks.Modify(timerLoad, static il =>
        {
            new ILCursor(il)
                .GotoNext(i => i.MatchNewobj<BardTimer>())
                .RemoveRange(3);
        });
        var timerUpdate = typeof(ThoriumPlayer).GetMethod(nameof(ThoriumPlayer.HandleBardBigInstrument), BindingFlags.Instance | BindingFlags.NonPublic);
        MonoModHooks.Add(timerUpdate, static (Action<ThoriumPlayer> orig, ThoriumPlayer self) =>
        {
            orig(self);
            self.bardTimer = self.bardTimerRight = true;
        });
    }
}
