import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserProfile } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.sass'],
})
export class ProfileComponent {
  user!: UserProfile;
  userId: any;
  form!: FormGroup;
  uploadedAvatar!: any;
  file: File | undefined;
  isChangeProfilePictureShown: boolean = true;
  minDate: Date;
  maxDate: Date;
  isUpdatingProfile: boolean = false;
  isUploadingProfilePicture: boolean = false;
  constructor(
    private accountService: AccountService,
    private formBuilder: FormBuilder
  ) {
    this.accountService.user.subscribe((x) => {
      if (x) {
        this.userId = x.id;
      }
    });
    const currentYear = new Date().getFullYear();
    this.minDate = new Date(currentYear - 100, 0, 1);
    this.maxDate = new Date(currentYear - 18, 11, 31);
    this.ngOninit();
  }
  ngOninit() {
    this.accountService.getById(this.userId).subscribe((x) => {
      if (x) {
        this.user = x;
        this.form = this.formBuilder.group({
          fullName: [this.user.fullName, Validators.required],
          bio: [this.user.bio, Validators.required],
          dateOfBirth: [this.user.dateOfBirth, Validators.required],
          isFemale: [this.user.isFemale, Validators.required],
        });
      }
    });
  }
  onSubmit() {
    this.isUpdatingProfile = true;
    this.accountService.update(this.userId, this.form.value).subscribe((x) => {
      if (x) {
        this.isUpdatingProfile = false;
      }
    });
  }
  changeProfilePicture() {
    this.isChangeProfilePictureShown = !this.isChangeProfilePictureShown;
  }
  onFilechange(event: any) {
    this.file = event.target.files[0];
  }

  upload() {
    this.isUploadingProfilePicture = true;
    if (this.file) {
      this.accountService.uploadfile(this.file).subscribe((resp) => {
        this.user = resp;
        this.isUploadingProfilePicture = false;
        this.changeProfilePicture();
      });
    } else {
      alert('Please select a file first');
      this.isUploadingProfilePicture = false;
    }
  }
}
