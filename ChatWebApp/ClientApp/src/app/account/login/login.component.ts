import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first, tap } from 'rxjs';
import { LoginUser } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.sass'],
})
export class LoginComponent {
  form!: FormGroup;
  loading = false;
  submitted = false;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private accountService: AccountService //private alertService: AlertService
  ) {}

  ngOnInit() {
    this.form = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  // convenience getter for easy access to form fields
  get f() {
    return this.form.controls;
  }

  onSubmit() {
    this.submitted = true;

    // reset alerts on submit
    //this.alertService.clear();

    // stop here if form is invalid
    if (this.form.invalid) {
      return;
    }

    this.loading = true;
    this.accountService
      .login(this.form.value)
      .pipe(
        tap(
          // Log the result or error
          {
            next: (data) => console.log(data),
            error: (error) => {
              console.log(error);
              this.loading = false;
            },
          }
        )
      )
      .subscribe(() => {
        this.router.navigate(['../home'], { relativeTo: this.route });
      });
  }
}
