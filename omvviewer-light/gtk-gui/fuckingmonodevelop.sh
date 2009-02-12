#!/bin/sh

find . -name "omvviewer*cs" -type f -exec sed -i 's/Gdk\.Pixbuf\.LoadFromResource/MainClass\.GetResource/g' {} \;
