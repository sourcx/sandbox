require 'rmagick'

image = Magick::Image.read("images/full_man.png").first

tie = Magick::Image.read("images/tie_2.png").first
image.composite!(tie, 0, 0, Magick::OverCompositeOp)


shirt = Magick::Image.read("images/shirt_0.png").first
image.composite!(shirt, 0, 0, Magick::OverCompositeOp)

suit = Magick::Image.read("images/suit_3.png").first
image.composite!(suit, 0, 0, Magick::OverCompositeOp)

image.write("images/out.png")
