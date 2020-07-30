import { Component, forwardRef, Injector, Input, OnInit, SimpleChange, SimpleChanges } from '@angular/core';
import { ControlComponentBase } from '@shared/common';
import { ClaimsServiceProxy } from '@service-proxies';
import { NzTreeNodeOptions } from 'ng-zorro-antd';
import { ArrayService } from '@delon/util';
import { finalize } from 'rxjs/operators';
import { NzTreeNode } from 'ng-zorro-antd/core/tree';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-permission-tree',
  templateUrl: './permission-tree.component.html',
  styleUrls: ['./permission-tree.component.less'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => PermissionTreeComponent),
    multi: true,
  }],
})
export class PermissionTreeComponent extends ControlComponentBase<string[]> implements OnInit {

  treeData: NzTreeNode[];

  constructor(
    injector: Injector,
    public arraySer: ArrayService,
    public claimsSer: ClaimsServiceProxy,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.loading = true;
    this.claimsSer.getAllClaimsWithTree()
      .pipe(finalize(() => {
        this.loading = false;
      }))
      .subscribe((res) => {
        this.treeData = this.arraySer.arrToTreeNode(res, {
          idMapName: 'claim',
          parentIdMapName: 'parent',
          titleMapName: 'claim',
        });
        debugger
      });
  }

  onAfterViewInit(): void {
  }

  onDestroy(): void {
  }

  onInit(): void {
  }

  onInputChange(changes: { [P in keyof this]?: SimpleChange } & SimpleChanges) {
  }

}
