# Singleplayer prestige beta

Available in **1.4.0-beta.1**, disabled by default. This beta adds isolated
singleplayer progression, permanent level tracking and a manual prestige reset.
Multiplayer prestige and configurable server balance are not included.

## Enable the experiment

1. Exit the game and back up its original save and any existing isolated profile.
2. Select **Singleplayer** in the launcher.
3. Add the following to the game folder's `server.ini`, then restart:

```ini
[progression]
isolate_singleplayer = 1
trace_enabled = 1
```

Enable **Diagnostics → Logging** in the launcher to collect observations in
`logs/ReRiff-engine-menu.log`. Tracing is optional; it does not enable prestige.

On first use, the experiment imports the existing native save once. It keeps
an account-specific local `reriff-sp-<hash>.bin` in the game's native DAT save
directory and a checkpoint bank under `ReRiff-progression/<profile>/`. It
requires an existing writable native save directory. Isolated sessions retain
the original serializer and do not replace the original Steam Cloud save.
Generated tracks and song caches keep their existing locations.

Set `isolate_singleplayer = 0` and restart to use the original shared save.
Progress earned in the experimental profile is not merged back automatically.
Back up or restore the **whole checkpoint bank and its working save together**;
do not mix a save from one generation with another generation's counters.

## Prestige

Earn all **38 original in-game achievements**, then open **Settings → Credits**.
The native confirmation asks whether to prestige. Accept to reset; Cancel or
Back preserves progress. If ineligible, Credits opens normally. Loading,
saving, completing achievements and restarting never prestige automatically.

A confirmed prestige clears coins, cycle XP, progression unlocks and the
original in-game achievements. It retains permanent main XP/level, the equipped
car and paint pack, race records, ghosts, song history and generated tracks.
Steam achievements remain earned. Progression unlocks use the original native
XP thresholds again from zero cycle XP.

Profile shows the native difficulty star beside the permanent level. The first
prestige uses a star alone, then x2, x3 and so on. It appears only on the level
section. First Prestige is stored in the originally hidden local achievement
slot; its logo-bearing achievement-list row is still pending.

## Rewards

Creator and race coin awards use **prestige count + 1**: ×1 initially, ×2 after
one prestige, ×3 after two. Quoted and credited amounts follow the same scoped
reward path. Purchases and unrelated credits are not multiplied.

Non-IAP paint packs use permanent-level discounts: 10% at level 10, 20% at 20,
25% at 30, then diminishing increases toward 30%. Free packs remain free and
paid packs cost at least one coin. Native signed counters are bounded.

## Validation and limits

Live testing verified the initial import, original-save isolation, native
race/save/restart persistence, a discounted purchase and restart, one confirmed
prestige, retention of permanent level/equipped items, cleared cycle progress,
and the native star's alignment and section visibility. A subsequent restart
retained the prestige checkpoint.

Automated tests cover reward boundaries, interrupted/reset transactions,
checkpoint corruption/domain checks, original native reset/ownership bindings,
confirmation refusal/cancellation and UI lifecycle. The extracted original-IL
fixture is created locally from a hash-verified original executable and is not
shipped. It is a test adaptation, not a byte-matching original reconstruction.

Live doubled creator/race payouts after prestige still need validation.
Power-loss durability is not established. A transaction failure stops gameplay
so startup can recover the last complete checkpoint. There is no multiplayer
authority or anticheat implementation in this beta.

Native research and its corrections are recorded in
[the recovery report](../reports/PRESTIGE-NATIVE-BOUNDARIES.md).
