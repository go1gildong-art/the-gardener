import pathlib
import re

base_dir = pathlib.Path(__file__).resolve().parent.parent
card_dir = base_dir / 'GardenerCode' / 'Cards'
files = [
    'BombBlossom.cs','BoughShield.cs','Fertilizer.cs','FungiRemover.cs','HostileNematode.cs',
    'LastDrop.cs','LeafShield.cs','LifeCocoon.cs','NutrientDump.cs','PollenStorm.cs',
    'PremiumSerum.cs','PyrrhicBlossom.cs','RapidGrowth.cs','VineStrike.cs','Vitality.cs',
    'Weeding.cs','WinterBreeze.cs','Wriggle.cs'
]

class_header_re = re.compile(
    r'public class (?P<name>\w+)\(\)\s*:\s*GardenerCode\.Cards\.GardenerCard\s*\((?P<args>.*?)\)(?P<suffix>\s*(?:,\s*[^\{]+?)?)\s*\{',
    re.S,
)

for filename in files:
    path = card_dir / filename
    text = path.read_text(encoding='utf-8')
    original = text

    m = class_header_re.search(text)
    if not m:
        print(f'NO CLASS HEADER MATCH: {filename}')
        continue

    name = m.group('name')
    args = m.group('args')
    suffix = m.group('suffix').rstrip()
    base_decl = f'public class {name} : GardenerCode.Cards.GardenerCard{suffix}'
    new_header = base_decl + '\n{' if suffix == '' else base_decl + '\n{'  # keep line break before brace
    text = text[:m.start()] + new_header + text[m.end():]

    if f'public int Nutrient' not in text:
        insert_point = text.find('{', m.end() - (len(new_header) + 1)) + 1
        constructor = (
            f'\n    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;\n\n'
            f'    public {name}() : base({args})\n'
            f'    {{\n'
            f'        NutrientModifier.AddTo(this, {{amount}});\n'
            f'    }}\n'
        )
        text = text[:insert_point] + constructor + text[insert_point:]

    nutrient_match = re.search(r'new IntVar\("Nutrient",\s*([^\)]+)\)', text)
    if nutrient_match:
        amount = nutrient_match.group(1).strip()
        text = re.sub(r'new IntVar\("Nutrient",\s*([^\)]+)\)', 'Nutrient', text, count=1)
        text = text.replace('{amount}', amount)
    else:
        print(f'NO NUTRIENT AMOUNT FOUND: {filename}')
        text = text.replace('{amount}', '0')

    text = re.sub(r'base\.DynamicVars\["Nutrient"\]\.UpgradeValueBy\(([^\)]+)\);', r'NutrientModifier.GetFrom(this)?.Increase(\1);', text)
    text = re.sub(r'DynamicVars\["Nutrient"\]\.UpgradeValueBy\(([^\)]+)\);', r'NutrientModifier.GetFrom(this)?.Increase(\1);', text)

    onplay_start = text.find('protected override async Task OnPlay(')
    if onplay_start != -1:
        brace_start = text.find('{', onplay_start)
        if brace_start != -1:
            depth = 0
            end_index = None
            for i in range(brace_start, len(text)):
                if text[i] == '{':
                    depth += 1
                elif text[i] == '}':
                    depth -= 1
                    if depth == 0:
                        end_index = i
                        break
            if end_index is not None:
                body = text[brace_start+1:end_index]
                match = re.search(r'(?P<indent>[ \t]*)(?P<calls>(?:await GardenerCmd\.ConsumeNutrient\(choiceContext, this\);\s*)+)\s*\Z', body, re.S)
                if match:
                    calls = match.group('calls')
                    count = len(re.findall(r'await GardenerCmd\.ConsumeNutrient\(choiceContext, this\);', calls))
                    indent = match.group('indent')
                    if count >= 2:
                        replacement = indent + 'await GardenerCmd.ConsumeNutrientNew(choiceContext, this);\n'
                    else:
                        replacement = ''
                    new_body = body[:match.start('calls')] + replacement
                    text = text[:brace_start+1] + new_body + text[end_index:]
    else:
        print(f'NO OnPlay METHOD: {filename}')

    if text != original:
        path.write_text(text, encoding='utf-8')
        print(f'UPDATED: {filename}')
    else:
        print(f'NO CHANGE: {filename}')
