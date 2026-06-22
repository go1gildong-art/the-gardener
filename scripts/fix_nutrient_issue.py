import pathlib
import re

base_dir = pathlib.Path(__file__).resolve().parent.parent
card_dir = base_dir / 'GardenerCode' / 'Cards'
files = [
    'LeafShield.cs','Fertilizer.cs','PollenStorm.cs','RapidGrowth.cs','BoughShield.cs',
    'BombBlossom.cs','FungiRemover.cs','HostileNematode.cs','PremiumSerum.cs','NutrientDump.cs',
    'PyrrhicBlossom.cs','LastDrop.cs','VineStrike.cs','Vitality.cs','WinterBreeze.cs','Wriggle.cs'
]

for filename in files:
    path = card_dir / filename
    text = path.read_text(encoding='utf-8')
    original = text

    class_name_match = re.search(r'public class (\w+)', text)
    if not class_name_match:
        print(f'NO CLASS NAME: {filename}')
        continue
    class_name = class_name_match.group(1)

    # Normalize class header if it still uses primary constructor syntax or has stray parentheses
    text = re.sub(rf'public class {class_name}\(\)\s*:\s*GardenerCode\.Cards\.GardenerCard', f'public class {class_name} : GardenerCode.Cards.GardenerCard', text)
    text = re.sub(rf'public class {class_name}\s*:\s*GardenerCode\.Cards\.GardenerCard\s*\(([^\)]*)\)', f'public class {class_name} : GardenerCode.Cards.GardenerCard', text)

    # Remove any existing Nutrient property definitions
    text = re.sub(r'\s*public int Nutrient => NutrientModifier\.GetFrom\(this\)\?\.Nutrient \?\? 0;\s*\n', '\n', text)

    # Remove any existing constructor definition for this class
    ctor_start = text.find(f'public {class_name}() : base(')
    if ctor_start != -1:
        open_brace = text.find('{', ctor_start)
        if open_brace != -1:
            depth = 0
            end = None
            for i in range(open_brace, len(text)):
                if text[i] == '{':
                    depth += 1
                elif text[i] == '}':
                    depth -= 1
                    if depth == 0:
                        end = i
                        break
            if end is not None:
                # remove constructor and trailing whitespace/newlines
                after = end + 1
                while after < len(text) and text[after] in ' \t\r\n':
                    after += 1
                text = text[:ctor_start] + text[after:]

    # Locate class opening brace
    class_open_match = re.search(rf'public class {class_name}[^\{{]*\{{', text)
    if not class_open_match:
        print(f'NO CLASS OPENING: {filename}')
        continue
    insert_point = class_open_match.end()

    # Determine constructor args from removed content if available by scanning original
    args_match = re.search(rf'public {class_name}\(\) : base\((.*?)\)\s*\{{', original, re.S)
    args = None
    if args_match:
        args = ' '.join(line.strip() for line in args_match.group(1).splitlines())
        args = re.sub(r'\s+', ' ', args).strip()

    addto_match = re.search(r'NutrientModifier\.AddTo\(this,\s*([^\)]+)\);', original)
    amount = addto_match.group(1).strip() if addto_match else '0'

    if args:
        snippet = (
            '\n    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;\n\n'
            f'    public {class_name}() : base({args})\n'
            '    {\n'
            f'        NutrientModifier.AddTo(this, {amount});\n'
            '    }\n\n'
        )
    else:
        # if args unknown, do not insert constructor with base
        snippet = '\n    public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;\n\n'

    # Only insert snippet if not already present at class top
    if 'public int Nutrient => NutrientModifier.GetFrom(this)?.Nutrient ?? 0;' not in text[:insert_point + 200]:
        text = text[:insert_point] + snippet + text[insert_point:]

    # Replace bare Nutrient in CanonicalVars with proper IntVar
    text = re.sub(r'(?m)^[ \t]*Nutrient,\s*$', '        new IntVar("Nutrient", Nutrient),', text)
    # Replace numeric IntVar Nutrient in CanonicalVars with dynamic nutrient reference
    text = re.sub(r'new IntVar\("Nutrient",\s*[^\)]+\)', 'new IntVar("Nutrient", Nutrient)', text)

    # Remove any accidental duplicated blank lines
    text = re.sub(r'\n{3,}', '\n\n', text)

    if text != original:
        path.write_text(text, encoding='utf-8')
        print(f'FIXED: {filename}')
    else:
        print(f'UNCHANGED: {filename}')
