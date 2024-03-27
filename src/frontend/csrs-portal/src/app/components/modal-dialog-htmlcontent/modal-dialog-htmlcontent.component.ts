import { Component, OnInit } from '@angular/core';
import { Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';

@Component({
  selector: 'app-modal-dialog',
  templateUrl: './modal-dialog-htmlcontent.component.html',
  styleUrls: ['./modal-dialog-htmlcontent.component.scss']
})
export class ModalDialogHtmlComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ModalDialogHtmlComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
  ) {}

  ngOnInit(): void {
  }

}
