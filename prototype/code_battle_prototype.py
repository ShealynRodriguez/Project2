"""
Code Battle - leveling prototype (terminal version)
Run with:  python3 code_battle_prototype.py

Structure: pick language -> pick starter -> 3 mini fights (catch to win)
-> boss fight. Every attack is a code challenge; wrong code = no damage
AND the enemy hits you back.
"""
import random

# ---------------------------------------------------------------- content
# Each challenge: (prompt with ___ blank, [accepted answers]).
# Add more here (or load from a JSON file) without touching game logic.
CHALLENGES = {
    "python": {
        "easy": [
            ("___('Hello')   # show text on screen", ["print"]),
            ("for i in ___(3):   # loop 3 times", ["range"]),
            ("nums = [1, 2, 3]\nprint(nums[___])   # first item", ["0"]),
        ],
        "medium": [
            ("def add(a, b):\n    ___ a + b   # send the answer back", ["return"]),
            ("n = ___(nums)   # how many items in the list", ["len"]),
            ("if x ___ 10:   # is x equal to 10?", ["=="]),
        ],
        "hard": [
            ("[n * n ___ n in range(5)]   # list comprehension", ["for"]),
            ("[n for n in nums ___ n % 2 == 0]   # keep only evens", ["if"]),
            ("word[::___]   # reverse a string", ["-1"]),
        ],
    },
    "javascript": {
        "easy": [
            ("___.log('Hello');   // print to the console", ["console"]),
            ("___ x = 5;   // declare a changeable variable", ["let", "var"]),
            ("if (x > 5) { a(); } ___ { b(); }   // otherwise", ["else"]),
        ],
        "medium": [
            ("function add(a, b) { ___ a + b; }   // send the answer back", ["return"]),
            ("let n = nums.___;   // how many items", ["length"]),
            ("for (let i = 0; i < 3; i___) { }   // add 1 each loop", ["++"]),
        ],
        "hard": [
            ("nums.___(n => n * 2)   // new array, every item doubled", ["map"]),
            ("const f = (a, b) ___ a + b;   // arrow function", ["=>"]),
            ("nums.___(n => n % 2 === 0)   // keep only evens", ["filter"]),
        ],
    },
}

DIFF = {"easy": {"dmg": 10, "xp": 5}, "medium": {"dmg": 20, "xp": 10}, "hard": {"dmg": 30, "xp": 15}}

# Rock-paper-scissors type chart: key beats value.
BEATS = {"fire": "grass", "grass": "water", "water": "fire"}
MOVES = {
    "fire": ["Ember", "Flame Wheel", "Inferno"],
    "water": ["Splash", "Aqua Jet", "Tidal Wave"],
    "grass": ["Vine Whip", "Razor Leaf", "Solar Beam"],
}
STARTERS = [("Emberpup", "fire"), ("Splashling", "water"), ("Leafkit", "grass")]

LEVELS = [
    {"name": "Mini fight 1", "mon": ("Cinderbat", "fire"),  "hp": 30,  "hits": 10, "boss": False},
    {"name": "Mini fight 2", "mon": ("Dripfin", "water"),   "hp": 45,  "hits": 12, "boss": False},
    {"name": "Mini fight 3", "mon": ("Mossback", "grass"),  "hp": 60,  "hits": 15, "boss": False},
    {"name": "BOSS FIGHT",   "mon": ("Chimera King", "fire"), "hp": 120, "hits": 20, "boss": True},
]
BOSS_PHASES = ["fire", "water", "grass"]  # boss changes type as it gets hurt
CATCH_WINDOW = 0.30                       # can throw a ball at <= 30% HP


# ---------------------------------------------------------------- helpers
def multiplier(attacker, defender):
    if BEATS[attacker] == defender:
        return 1.5
    if BEATS[defender] == attacker:
        return 0.5
    return 1.0


def gain_xp(state, amount):
    state["xp"] += amount
    while state["xp"] >= state["level"] * 30:
        state["xp"] -= state["level"] * 30
        state["level"] += 1
        state["max_hp"] += 10
        print(f"  *** LEVEL UP! You are now level {state['level']} (max HP {state['max_hp']}) ***")


def ask(state, difficulty):
    """Show a code challenge. Returns (correct, used_hint)."""
    prompt, answers = random.choice(CHALLENGES[state["lang"]][difficulty])
    print(f"\n  [{difficulty.upper()}] Fill in the blank:\n")
    for line in prompt.split("\n"):
        print("     " + line)
    used_hint = False
    while True:
        reply = input("  > ").strip().lower()
        if reply == "hint":
            if state["hints"] > 0 and not used_hint:
                state["hints"] -= 1
                used_hint = True
                a = answers[0]
                print(f"  Hint: starts with '{a[0]}', {len(a)} characters (damage halved)")
            else:
                print("  No hints left this fight.")
            continue
        return reply in answers, used_hint


def pick(options, label):
    for i, o in enumerate(options, 1):
        print(f"  {i}. {o}")
    while True:
        c = input(f"{label} > ").strip()
        if c.isdigit() and 1 <= int(c) <= len(options):
            return int(c) - 1
        print("  Pick a number from the list.")


# ---------------------------------------------------------------- fight
def fight(state, level):
    name, etype = level["mon"]
    ehp = max_ehp = level["hp"]
    boss = level["boss"]
    state["hp"] = state["max_hp"]
    state["hints"] = 1
    print(f"\n{'=' * 50}\n{level['name']}: a wild {name} ({etype}) appears!\n{'=' * 50}")

    while True:
        if boss:
            phase = min(2, int((max_ehp - ehp) // 40))
            etype = BOSS_PHASES[phase]
        print(f"\n{name} [{etype}] HP {max(int(ehp), 0)}/{max_ehp}   |   "
              f"You: HP {state['hp']}/{state['max_hp']}  Lv {state['level']}  hints {state['hints']}")

        # Catch window (mini fights only)
        if not boss and ehp <= max_ehp * CATCH_WINDOW:
            if input("  It's weak! Throw a catch ball? (y/n) > ").lower().startswith("y"):
                ok, _ = ask(state, "medium")
                if ok:
                    print(f"  Gotcha! {name} was caught!")
                    state["team"].append((name, etype))
                    gain_xp(state, 20)
                    return True
                print("  It broke free!")
                state["hp"] -= level["hits"]
                print(f"  {name} hits you for {level['hits']}.")
                if state["hp"] <= 0:
                    return False
                continue

        print("\n  Who attacks?")
        member = state["team"][pick([f"{n} ({t})" for n, t in state["team"]], "Pokemon")]
        print("\n  Choose a move (harder code = more damage):")
        tier = pick([f"{MOVES[member[1]][i]}  [{d}, {DIFF[d]['dmg']} base dmg]"
                     for i, d in enumerate(DIFF)], "Move")
        diff = list(DIFF)[tier]

        ok, hinted = ask(state, diff)
        if ok:
            mult = multiplier(member[1], etype)
            dmg = DIFF[diff]["dmg"] * mult * (0.5 if hinted else 1)
            ehp -= dmg
            note = " Super effective!" if mult > 1 else " Not very effective..." if mult < 1 else ""
            print(f"  Correct! {MOVES[member[1]][tier]} deals {int(dmg)} damage.{note}")
            gain_xp(state, DIFF[diff]["xp"])
            if not boss:
                ehp = max(ehp, 1)  # mini-fight enemies can't be KO'd, only caught
            elif ehp <= 0:
                print(f"  {name} is defeated!")
                return True
        else:
            print(f"  Compile error! No damage. {name} hits you for {level['hits']}.")
            state["hp"] -= level["hits"]
            if state["hp"] <= 0:
                return False


# ---------------------------------------------------------------- main
def main():
    print("CODE BATTLE (prototype)\n")
    print("Choose your coding language:")
    langs = list(CHALLENGES)
    lang = langs[pick(langs, "Language")]
    print("\nChoose your starter:")
    starter = STARTERS[pick([f"{n} ({t})" for n, t in STARTERS], "Starter")]
    state = {"lang": lang, "team": [starter], "level": 1, "xp": 0,
             "max_hp": 50, "hp": 50, "hints": 1}

    for level in LEVELS:
        while not fight(state, level):  # checkpoint: lose = retry same fight
            print("\n  You blacked out! Retrying the fight...")
            state["hp"] = state["max_hp"]
        print(f"\n  {level['name']} cleared!")

    print(f"\nVICTORY! Team: {', '.join(n for n, _ in state['team'])}. Final level {state['level']}.")


if __name__ == "__main__":
    main()
