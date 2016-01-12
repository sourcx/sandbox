package nl.fevers.app;
import com.tinify.*;
import java.io.IOException;

public class Compress {
    public static void main(String[] args) throws java.io.IOException {
        Tinify.setKey(System.getenv("TINY_KEY"));
        System.out.println("Key: " + System.getenv("TINY_KEY"));
        System.out.println("Compression-Count: " + Tinify.compressionCount());

        // File
        Tinify.fromFile("../images/in/small.png").toFile("../images/out/small-compressed-url.png");
        System.out.println("Compression done. Compression-Count: " + Tinify.compressionCount());

        // URL
        Source tiny = Tinify.fromUrl("https://raw.githubusercontent.com/tinify/tinify-ruby/master/test/examples/voormedia.png");
        tiny.toFile("../images/out/small-compressed-url.png");
        System.out.println("Compression done. Compression-Count: " + Tinify.compressionCount());
    }
}