import { Component, Inject } from '@angular/core';
import { Conversation, ConversationInfoDto } from 'src/app/_models/conversation';
import {MatDialog, MAT_DIALOG_DATA, MatDialogModule} from '@angular/material/dialog';
import UtilsService from 'src/app/_helpers/util';

@Component({
  selector: 'app-default-dialog',
  templateUrl: './conversation-detail-dialog.component.html',
  styleUrls: ['./conversation-detail-dialog.component.sass']
})
export class ConversationDetailDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: ConversationInfoDto) {
  }
  isEmpty(array: any): boolean {
    return UtilsService.isEmpty(array);
  }
}
