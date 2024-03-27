import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ModalDialogHtmlComponent } from '../../../components/modal-dialog-htmlcontent/modal-dialog-htmlcontent.component';


@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent implements OnInit {

  constructor(public dialog: MatDialog,) { }

  ngOnInit(): void {
  }
  showTermsOfUse(): void {
    const dialogRef = this.dialog.open(ModalDialogHtmlComponent, {
      width: '750px',
      data: '',
    });

    dialogRef.afterClosed().subscribe(result => {
      //this.logger.info(`Dialog result: ${result}`);
    });
  }
}
