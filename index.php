<?php
session_start();
ob_start();
 ?>

<!DOCTYPE HTML>
<html>
	<?php include "inc/site/meta.php" ?>

		<title>Gıda</title>
	</head>
	<body class="homepage is-preload">
		<div id="page-wrapper">
			<!-- Header -->
				<?php $headerType = 1; $headerText = 'Gıda'; $url = '/';

        include "inc/site/header1.php";

        include "inc/about/about1.php";

        include "inc/portfolio/portfolio1.php";

        include "inc/features/features1.php";

        include "inc/contact/contactForm1.php";

        include "inc/site/footer1.php";

			  ?>
		</div>

		<!-- Scripts -->
			<?php include "inc/site/scripts.php" ?>

	</body>
</html>
