# 🎡 Archipelago Parkitect

Welcome to the **Archipelago Parkitect Mod**!  
This mod connects **Parkitect** to the **Archipelago** multiworld randomizer — allowing for a unique and interconnected experience across multiple games.  

It’s also a **randomizer** in its own right! 🌀

---

## 📦 Installation

1. **Download the Mod Files**
    - You can find the latest release [here](https://github.com/CrusherRL/AP_Parkitect/releases).

2. **Extract the Files**
    - Place the extracted folder into your Parkitect `Mods` directory.  
      Windows:  
      ```
      ...\Documents\Parkitect\Mods\Archipelago
      ```
      MacOs:  
      ```
      ~/Library/Application Support/Parkitect/Mods/Archipelago
      ```
      Linux:  
      ```
      /home/[username]/.local/share/Steam/steamapps/common/Parkitect/Mods
      ```

3. **Enable the Mod**
    - Launch Parkitect.
    - Go to **Main Menu → Mods**.
    - Enable **Archipelago**.

4. **Download the Campaign** (preferably manually, mod.io is weird sometimes)
    - Place the **Archipelago.campaign** file into `\Documents\Parkitect\Saves\Campaigns`
    - If you insist on mod.io, you can find it [here](https://mod.io/g/parkitect/m/archipelago-parks#description)

5. **Start the Game** and enter corresponding Park
   - On Main Menu click on Campaign
     - Top left (Campaign Maps) -> Community Campaigns -> "Archipelago Parks"

6. **Configure your Config**
   - If the mod can’t find the config file, it will automatically create one.
   - If you don’t see the folder yet, start Parkitect once with the mod enabled — it will generate the config automatically.
   - Anyway you can find it there:
     - Windows:
      ```
      ...\AppData\LocalLow\Texel Raptor\Parkitect\Parkitect_Archipelago\config_parkitect.json
      ```
     - MacOS:
      ```
      ~/Library/Application Support/com.TexelRaptor.Parkitect/Parkitect_Archipelago/config_parkitect.json
      ```
     - Linux:
      ```
      /home/[username]/.config/unity3d/Texel Raptor/Parkitect/Parkitect_Archipelago
      ```
   
    ```json
    {
        "Address": "localhost",
        "Port": 38281,
        "Playername": "CrusherParkitect",
        "Password": ""
    }
    ```
    - Once configured you should restart your Park. Preferably you should quit the current park and re-enter it. In this way all Mods are reloading.

7. **Connect to Archipelago**
   - This mod does not need an extra Client to connect to the Archipelago Server

---

## 🖥️ Archipelago UI Overview
Shortcut to toggle this Window is **Z**

![Archipelago UI Window](Src/Images/ArchipelagoMod.png "UI Window")

### Debugger Window
If something doesn’t work as expected, press F12 to open the Debugger Window for detailed logs and troubleshooting.

---

## 🔀 What Can the Randomizer Change?

### 🎢 Player
- Adjust **Game Speed** (adds new options: `4x`, `5x`, `6x`, `7x`, `7x, `9x`)
- Add **Money**

### 🧍 Guests
- Spawn guests
- Modify their money (+/-)
- Kill (remove) them
- Change their needs (hungry, thirsty, happy, tired, bathroom)
- Cause **vomiting**/**nausea**
- Turn them into **vandals**

### 🧑‍🔧 Employees
- Hire automatically
- Set **tired** state
- Send for **training**

### ☁️ Weather
- Set to **Rainy**, **Cloudy**, **Sunny** or **Stormy**

### 🎠 Attractions
- Trigger **breakdowns**
- Apply **vouchers**

### 🍔 Stalls / Shops
- Re-deliver ingredients
- Set **cleaning tasks**
- Apply **vouchers**

### 🗺️ Scenario
- Add any **goal** with any **reward**

---

## 🎯 Goals
Win by completing the park/scenario goals — or by finishing all challenges/checks.

### Parkitect Goals
You can configure multiple win conditions for your scenario. Each goal can be adjusted individually:

#### 👥 Guest Goal
Defines how many guests must be in your park to win.  
- **Range:** 1 – 2500  
- **Default:** 1000  

#### 💰 Money Goal
Requires reaching a certain amount of money.  
- **Range:** 50,000 – 500,000  
- **Default:** 100,000  

#### 🎢 Roller Coaster Goal
Number of roller coasters required.  
- **Range:** 0 – 12  
- **Default:** 4  

#### 😀 Excitement Rating
Minimum excitement required for a coaster to count.  
Set to **0** to disable this requirement.  
- **Range:** 0 – 80  
- **Default:** 50  

#### 😬 Intensity Rating
Minimum intensity required for a coaster to count.  
Set to **0** to disable this requirement.  
- **Range:** 0 – 80  
- **Default:** 50  

#### 📈 Ride Profit Goal
Total profit required from all rides.  
- **Range:** 0 – 10,000  
- **Default:** 1500  

#### 🎟️ Park Tickets Goal
Number of park tickets that must be sold.  
- **Range:** 0 – 20,000  
- **Default:** 0  

#### 🏪 Shops Goal
Number of shops required in the park.  
- **Range:** 0 – 100  
- **Default:** 30  

#### 💵 Shop Profit Goal
Total profit required from shops.  
- **Range:** 0 – 3,000  
- **Default:** 500  

---

## 🎯 Challenge/Check Requirements

### 🎡 Attraction Challenge/Check
To complete:
- Must be **open**
- Stats must **not be outdated**
- Must have had **at least one customer**

### 🍟 Shop Challenge/Check
To complete:
- Must be **open**
- Must have had **at least one customer**

---

## 🧪 Testing & Compatibility

| Category | Status | Notes |
|-----------|---------|-------|
| **Operating Systems** | ✅ Tested on **Windows 10** and **MacOS Sequoia 15.6** and **Linux** ||
| **Multiplayer** | ⚠️ Not Tested / Likely Unsupported | The mod was designed for single-player mode — multiplayer may cause sync issues |
| **Game Version** | ✅ Steam release (1.12e) | Earlier versions before Steam release (1.12b2) won’t work |
| **Other Mods** | ⚙️ Attraction/Shop Mods are Supported ([Steam Collection](https://steamcommunity.com/sharedfiles/filedetails/?id=3647109901)) ||
| **Performance** | ✅ Stable | No major FPS drops or memory issues during extended play |
| **Archipelago Connection** | ✅ Tested with local and remote servers | No known connection issues |
| **Archipelago Multigame** | ✅ Stable ||

> If you find new issues, please report them (see below).

---

## 🧾 Reporting Bugs & Feedback

If you encounter issues or have suggestions:

1. Open a new issue on [GitHub](https://github.com/CrusherRL/AP_Parkitect_World/issues)
2. Include:
   - A short **description** of the problem  
   - Your **log file** (found under `Mods/Archipelago/debug.log.text`)  
   - Any **screenshots** if relevant
   - (Optional) The SlotData file
3. Tag it appropriately:
   - `bug` for errors or crashes  
   - `enhancement` for ideas or improvements  

---

## 🚫 Things You Shouldn’t Do

- Don’t try to **run multiple Archipelago connections** at once.
- Don’t **rename** or move internal mod files.
- Don’t **edit save data manually** — it may break synchronization.
- Don’t **overwrite slotdata files** from other worlds.

---

## 🧭 Scenario Rules (for Contributors)

> 💡 You can **request maps** if you’d like to contribute!

To be accepted, a scenario **must**:
- Include **all attractions and shops** except **Mod Items**
  - Decorations are optional, they will not be randomized
- Have **1 mandatory goal** (usually 95% Happiness)
- Allow **guests to enter** the park without path issues
- Be **fun and engaging** (no empty maps 😅)
- Be **possible** but not overly difficult
- Enough **Space** to build a lot of stuff 
- The Park **must** work in vanilla

---

## 🏗️ Submitting New Parks

Want your park to be part of the Archipelago experience?

To submit a park:
1. Follow the **Scenario Rules** above.
2. Export your scenario and send it via:
   - GitHub Pull Request, or
   - [Discord](https://discord.com/channels/731205301247803413/1417531615956963439)

---

## 💡 Future Ideas

- add a dedicated **Archipelashop** building:
  - receives items at the **Depot**, delivered by **Handyman**
  - unlocks new features when deliveries are complete
- add more diverse **Scenarios**
- improve **Scenario Design**
- improve **Shop Ingredient Trap**
- enhance the **UI Window** (fade animations)

---

## 🌍 Related Projects

- [AP_Parkitect_World](https://github.com/CrusherRL/AP_Parkitect_World)

---

🧩 Have fun building and randomizing!  
Every park tells a story — yours just happens to be shared across worlds.

---

## ❤️ Credits

Created by **CrusherRL**
Special thanks to my friends who helped me with ideas, debugging and adding content!
Special thanks to the **Archipelago Community** for testing, feedback, and support!
