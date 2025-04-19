## Plugin Configuration
```
<?xml version="1.0" encoding="utf-8"?>
<LockpickChanceConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Logging>true</Logging>
  <Stealies>
    <StealyEntry>
      <ItemId>1353</ItemId>
      <LockpickChance>50</LockpickChance>
    </StealyEntry>
  </Stealies>
</LockpickChanceConfiguration>
```
## Logging
The `<Logging>` is by default set to true, this enables you to choose if you want the plugin related logs to appear in your Rocket.log and your Console. This feature sadly cannot be turned off at the time being and will be fixed in a future patch.
## Stealies
The `<Stealies>` section defines the list of all of your stealies/lockpicks, you will have to mannually add individual ones that your mods include, but by default the vanilla Stealy will be included with a default chance of 50% to succeed.
## Stealy Entry
The `<StealyEntry>` will be the section in which you will define the item ID of your stealy/lockpick and the chance of it succeeding when used.

