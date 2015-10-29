<?
$key = "<YOUR KEY>";
$input = "large-input.jpg";
$output = "tiny-output.jpg";

$url = "https://api.tinify.com/shrink";
$options = array(
  "http" => array(
    "method" => "POST",
    "header" => array(
      "Content-type: image/png",
      "Authorization: Basic " . base64_encode("api:$key")
    ),
    "content" => file_get_contents($input)
  ),
  "ssl" => array(
    /* Uncomment below if you have trouble validating our SSL certificate.
       Download cacert.pem from: http://curl.haxx.se/ca/cacert.pem */
    // "cafile" => __DIR__ . "/cacert.pem",
    "verify_peer" => true
  )
);

$result = fopen($url, "r", false, stream_context_create($options));
if ($result) {
  /* Compression was successful, retrieve output from Location header. */
  foreach ($http_response_header as $header) {
    if (strtolower(substr($header, 0, 10)) === "location: ") {
      file_put_contents($output, fopen(substr($header, 10), "rb", false));
    }
  }
} else {
  /* Something went wrong! */
  print("Compression failed");
}
?>