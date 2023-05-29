import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { map } from 'rxjs';
import { User, UserProfile } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.sass'],
})
export class ProfileComponent {
  user!: UserProfile;
  form!: FormGroup;
  uploadedAvatar!: any;
  constructor(
    private accountService: AccountService,
    private formBuilder: FormBuilder
  ) {
    this.accountService.user.subscribe((x) => {
      if (x) {
        this.user = x;
      }
    });
    this.ngOninit();
  }
  ngOninit() {
    this.form = this.formBuilder.group({
      fullName: [this.user.fullName, Validators.required],
      bio: [this.user.bio, Validators.required],
    });
  }
  onSubmit() {}
  uploadAvatar() {
    console.log(this.uploadedAvatar);
    this.accountService
      .submitAvatar(this.uploadedAvatar)
      .pipe(
        map((res) => {
          console.log(res);
        })
      )
      .subscribe();
  }
  onFileSelected(event: Event) {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
      console.log(target.files[0].name);
      this.uploadedAvatar = target.files[0];
    }
  }
}
