var togglePassword = document.getElementById("toggle-password");

if (togglePassword) {
	togglePassword.addEventListener('click', function() {
	  var x = document.getElementById("Password");
	  if (x.type === "password") {
	    x.type = "text";
	  } else {
	    x.type = "password";
	  }
	});
}

var togglePassword1 = document.getElementById("toggle-password1");

if (togglePassword1) {
	togglePassword1.addEventListener('click', function () {
		var x = document.getElementById("Password");
		if (x.type === "password") {
			x.type = "text";
		} else {
			x.type = "password";
		}

		var y = document.getElementById("PassaportAgain");
		if (y.type === "password") {
			y.type = "text";
		} else {
			y.type = "password";
		}
	});
}