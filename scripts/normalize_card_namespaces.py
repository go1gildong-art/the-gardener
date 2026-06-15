import pathlib
import re

base_dir = pathlib.Path(__file__).resolve().parent.parent
card_dir = base_dir / 'GardenerCode' / 'Cards'
pattern_file = re.compile(r'namespace Gardener;')
header_pattern = re.compile(r'public class (\w+)(\s*\(\))?\s*:\s*GardenerCode\.Cards\.GardenerCard')

for path in sorted(card_dir.glob('*.cs')):
    text = path.read_text(encoding='utf-8')
    original = text

    text = pattern_file.sub('namespace Gardener.GardenerCode.Cards;', text)
    text = header_pattern.sub(r'public class \1\2 : GardenerCard', text)

    if text != original:
        path.write_text(text, encoding='utf-8')
        print(f'UPDATED: {path.name}')
    else:
        print(f'UNCHANGED: {path.name}')
