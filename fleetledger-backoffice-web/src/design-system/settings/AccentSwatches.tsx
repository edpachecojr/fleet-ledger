import { Check } from 'lucide-react'
import type { ReactNode } from 'react'
import { ACCENT_SWATCHES, type Accent } from '../theme/themeSettings'

interface AccentSwatchesProps {
  selected: Accent
  onSelect: (accent: Accent) => void
}

export function AccentSwatches({ selected, onSelect }: AccentSwatchesProps): ReactNode {
  return (
    <div className="flex flex-wrap gap-3">
      {ACCENT_SWATCHES.map(({ accent, swatch, label }) => (
        <button
          key={accent}
          type="button"
          aria-label={label}
          aria-pressed={selected === accent}
          onClick={() => {
            onSelect(accent)
          }}
          style={{ background: swatch, color: swatch }}
          className="grid size-8 place-items-center rounded-full aria-pressed:shadow-[0_0_0_2px_var(--surface),0_0_0_4px_currentColor]"
        >
          {selected === accent && <Check className="size-4 text-white" aria-hidden />}
        </button>
      ))}
    </div>
  )
}
