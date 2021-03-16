# Sonic Generations Configuration Tool (Rewrite)

Rewriting Sonic Generations' Configuration Tool using WPF as a way to teach myself how to use it.
Font images courtesy of [M&M](https://github.com/ActualMandM)

Current features:

- Supports Graphics, Audio and Analytics configuration
- Supports fixing the registry to avoid the `Game files either missing or corrupt please reinstall` issue
- Supports proper scaling
- Includes settings descriptions and images to better visualize the impact of each one
- Removes the need for DefaultInput.cfg in the game's directory
- Ability to map keys that couldn't be mapped in the original config tool (e.g. TAB, Shift, CTRL...)

Current drawbacks:
- Still no proper Nvidia Optimus support, just like the original configuration tool
- No Dinput devices support, but since Sonic Generations doesn't properly support Dinput to begin with, it shouldn't matter that much
