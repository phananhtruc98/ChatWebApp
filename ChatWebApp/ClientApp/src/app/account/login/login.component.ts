import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { tap } from 'rxjs';
import { AccountService } from 'src/app/_services/account.service';
import { SignalRService } from 'src/app/_services/signalr.service';
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
    private accountService: AccountService, //private alertService: AlertService
    private _signalrService: SignalRService,
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

    if (this.form.invalid) {
      return;
    }

    this.loading = true;
    this.accountService
      .login(this.form.value)
      .pipe(
        tap(
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
