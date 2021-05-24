import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MomentFormatPipe, MomentFromNowPipe } from './/moment-format';

export const PIPES = [MomentFormatPipe, MomentFromNowPipe];

@NgModule({
  imports: [CommonModule],
  declarations: [...PIPES],
  exports: [...PIPES],
})
export class PipeModule {
  constructor() {}
}
