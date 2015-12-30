<?php
require_once("vendor/autoload.php");
Tinify\setKey(getenv("TINY_KEY"));

echo "COMPRESSION BY URL\n";
$tiny = Tinify\fromFile("images/in/small.png");
$tiny->toFile("images/out/small-compressed.png");
echo "Compression done, thank you come again!\n";
echo "Count: " . Tinify\getCompressionCount() . "\n";

echo "COMPRESSION BY UPLOAD\n";
$tiny = Tinify\fromUrl("https://raw.githubusercontent.com/tinify/tinify-ruby/master/test/examples/voormedia.png");
$tiny->toFile("images/out/small-compressed-url.png");
echo "Compression done, thank you come again!\n";
echo "Count: " . Tinify\getCompressionCount() . "\n";

?>