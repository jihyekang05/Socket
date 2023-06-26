using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using EumSolutions.Common.EumForm;
using EumSolutions.DataTransfer;
using EumSolutions.Common;
using DevExpress.XtraSplashScreen;
using KIS.DEV.COMMON;
using KITS.KIS.DBCOMMON;
using KITS.KIS.COMMON;
using EumSolutions.DevEx.Grid;
using EumSolutions.DataTransfer.Common;
using EumSolutions.DevEx.Lookup;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.Data.Mask;
using EumSolutions.DevEx.Editor;
using System.Globalization;
using DevExpress.Utils;
using DevExpress.XtraGrid;


namespace KITS.IX600000
{
    public partial class FrmIX603000 : FrmMDIMaster

    {
        //DATA 전송 모듈
        CEumDataTransfer oDataTransfer;
        

        //초기화 변수
        private bool isInit = true;
        //행추가 카운트 변수
        private int addcol = 0;

        private string innertext = "";

        string prev_cbo = "";

        string input_under_data = "";
        //색깔
        private Color COLOR_LIGHTRED = Color.FromArgb(255, 230, 230);
        private Color COLOR_BLUE = Color.FromArgb(153, 204, 255);

        //메뉴ID
        private const string MENU_ID = "IX603000";

        public FrmIX603000()
        {
            InitializeComponent();
            oDataTransfer = new CEumDataTransfer(CEumConstant.EUM_KEY_VALUE, CEumConstant.EUM_ENCODING);
        
        }

        /// 폼 로드
        private void FrmIX603000_Load(object sender, EventArgs e)
        {
            timerInit.Start();
            
        }
       

        //타이머 이벤트
        private void timerInit_Tick(object sender, EventArgs e)
        {
            

            try
            {
                SplashScreenManager.ShowForm(this, typeof(FrmLoading));


                timerInit.Stop();
                timerInit.Enabled = false;


                if (isInit)
                {
                    //화면값 초기화
                    InitForm();

                    SetGrid();
                }
                isInit = false;               
                                
            }
            catch (Exception ex)
            {
                CEumCommon.ShowEumMessageBox(ex, this.Name, this.Text);
            }
            finally
            {
                SplashScreenManager.CloseForm();
            }
        }

        /// 폼 초기화
        private void InitForm()
        {
            cboType.Properties.NullText = "선택";

            //조회기간
            dateToDate.EditValue = DateTime.Today;
            dateFromDate.EditValue = DateTime.Today.AddDays(-7);
            
            gvList.IndicatorWidth = 50;

           
            //입력 콤보박스 세팅
            ComboUtil.SetCombo(oDataTransfer, cboType, "EC909", "CODE", "S", false);
            cboType.Properties.NullText = "선택";

            
        }

        /// 그리드 출력(초기화)
        private void SetGrid()
        {
            string baseDate = CEumCommon.GetDateEditorValue(dateFromDate);
            string ToDate = CEumCommon.GetDateEditorValue(dateToDate);
            string s4 = CEumCommon.GetDate(CEumEnum.DATE_FORMAT.CS_YYYYMMDD, ToDate, "-");
            
            string sInitYn = "";

            //행추가 한줄만 가능하도록 하기 위한 변수
            addcol = 0;

            if (isInit == true)
            {
                sInitYn = "1";

            }

            

            //Oracle Procedure
            string sSp = "PROC_KITS_IX603000_S01";

            //Procedure Parameter
            object[] oParam = new object[4];

           
            oParam[0] = new object[] { "i_init_yn", "V", sInitYn };
            oParam[1] = new object[] { "i_base_date", "V", baseDate.Substring(0,8).ToNullSafeString() };
            oParam[2] = new object[] { "i_to_date", "V", s4.ToNullSafeString() };
            oParam[3] = new object[] { "i_under_data_div", "V", CEumCommon.GetObjToStr(cboType.EditValue.ToNullSafeString())};
            
            
       
            //그리드 바인딩
            DataSet dsData = oDataTransfer.GetSPDataSetGrid(CEumDataTransfer.EUM_DB_TYPE,
                                                                        CEumDTEnum.DB_CALL_TYPE.PROC,
                                                                        sSp,
                                                                        oParam,
                                                                        true,
                                                                        CEumConstant.ES_UID,
                                                                        "",
                                                                        "IX603000");


            CEumGrid.DrawDynamicGridView(gvList, dsData, sSp);
            
            GridCommon.SetColumnFont(gvList);
            GridCommon.SetGridLineColor(gvList, Color.DarkGray);


           
            
            gvList.Columns["INPUT_MAN_NAME"].AppearanceCell.BackColor = Color.Gainsboro;
            gvList.Columns["INPUT_DTIME"].AppearanceCell.BackColor = Color.Gainsboro;
            gvList.Columns["BASE_DATE"].AppearanceCell.BackColor = Color.Gainsboro;
            gvList.Columns["END_DATE"].AppearanceCell.BackColor = Color.Gainsboro;

            
           
            //조회된 값 수정불가            
            gvList.Columns[0].OptionsColumn.AllowEdit = false;
            gvList.Columns[1].OptionsColumn.AllowEdit = false;
            gvList.Columns[2].OptionsColumn.AllowEdit = false;
            gvList.Columns[3].OptionsColumn.AllowEdit = false;
            gvList.Columns[4].OptionsColumn.AllowEdit = false;
            gvList.Columns[5].OptionsColumn.AllowEdit = false;
            gvList.Columns[6].OptionsColumn.AllowEdit = false;


            
        
        }

        //조회버튼 이벤트
        private void btnSearch_Click(object sender, EventArgs e)
        {
            
            SetGrid();
        
        }

        //저장버튼 이벤트
        private void btnSaveBS_Click(object sender, EventArgs e)
        {
            
              try
                {
                    for (int row = 0; row < 1; row++)
                    {
                       
                        
                     //체크된 값일 때
                        if (gvList.GetRowCellValue(row, "CHK").ToNullSafeString() == "1" )
                        {
                            //체크도 되어있고 금리값이 null이 아닐 때
                            if (gvList.GetRowCellValue(row, "UNDER_DATA").ToNullSafeString() != "")
                            {
                                //입력한 금리값
                                input_under_data = gvList.GetRowCellValue(row, "UNDER_DATA").ToNullSafeString();

                                //체크되어있는 값 저장하려고 하는데 이미 오늘일자는 입력을 했을 때( 프로시저 타면 안된다.)
                                if (gvList.GetRowCellValue(1, "INPUT_DTIME").ToNullSafeString().Substring(0, 10) == DateTime.Now.ToShortDateString())
                                {
                                    CEumCommon.ShowEumMessageBox("오늘 금리값을 입력하셨습니다.(수정 요청은 IT팀 문의 바람)");
                                    SetGrid();
                                }
                                else
                                {
                                    if (gvList.GetRowCellValue(1, "UNDER_DATA").ToNullSafeString() == gvList.GetRowCellValue(row, "UNDER_DATA").ToNullSafeString())
                                    {
                                        CEumCommon.ShowEumMessageBox("이 전 금리값과 같아서 입력자와 입력시간만 업데이트 됩니다.");
                                    }

                                    // Oracle Procedure
                                    string sSP = "PROC_KITS_IX603000_I01";

                                    // Procedure Parameter
                                    object[] oParam = new object[5];
                                    oParam[0] = new object[] { "i_under_data_div", "V", gvList.GetRowCellValue(row, "INDEX_DIV").ToNullSafeString().Substring(0, 2) };//구분
                                    oParam[1] = new object[] { "i_base_date", "V", gvList.GetRowCellValue(row, "BASE_DATE").ToNullSafeString() };//기준일자
                                    oParam[2] = new object[] { "i_to_date", "V", gvList.GetRowCellValue(row, "END_DATE").ToNullSafeString() };//기준종료일
                                    oParam[3] = new object[] { "i_under_data", "V", gvList.GetRowCellValue(row, "UNDER_DATA").ToNullSafeString() };//금리
                                    oParam[4] = new object[] { "i_input_man_id", "V", CEumConstant.ES_UID };//입력자

                                    DataTable dt = oDataTransfer.GetSPDataTable(CEumDataTransfer.EUM_DB_TYPE,
                                                                                CEumDTEnum.DB_CALL_TYPE.PROC,
                                                                                sSP,
                                                                                oParam,
                                                                                CEumConstant.ES_UID,
                                                                                "",
                                                                                "IX603000");
                                    if (dt.Rows.Count > 0)
                                    {
                                        if (dt.Rows[0]["R_CODE"].ToString() == "SUCC")
                                        {
                                            gvList.SetRowCellValue(row, "CHK", "0");
                                        }
                                        else
                                        {
                                            gvList.SetRowCellValue(row, "CHK", "1");
                                        }
                                    }



                                    CEumCommon.ShowEumMessageBox("저장완료");
                                    SetGrid();
                                }
                            }
                             //체크는 되어있지만 금리값은 null일 때
                            else
                            {
                                CEumCommon.ShowEumMessageBox("금리값을 입력하세요");
                            
                            }
                        }
                       
                        
                    }
                }
                catch (Exception ex)
                {
                    CEumCommon.ShowEumMessageBox(ex.Message);
                }
                finally
                {
                    
                    
                }
            
           
        }


        //temp
        private void gvList_RowUpdated(object sender, DevExpress.XtraGrid.Views.Base.RowObjectEventArgs e)
        {
            if (gvList.OptionsView.NewItemRowPosition == NewItemRowPosition.Top)
                gvList.OptionsView.NewItemRowPosition = NewItemRowPosition.None;
        }
        

        private void dtBaseDate_EditValueChanged(object sender, EventArgs e)
        {
           
        }

        //콤보박스
        private void cboMakePg_EditValueChanged(object sender, EventArgs e)
        {

        }

       


        //행추가 버튼 이벤트
        private void btnAddRow_Click(object sender, EventArgs e)
        {
            
            //중간에 구분 값 바뀔 시 작동하는 부분
            if (prev_cbo != CEumCommon.GetObjToStr(cboType.EditValue.ToNullSafeString()))
            {
                SetGrid();
                prev_cbo = CEumCommon.GetObjToStr(cboType.EditValue.ToNullSafeString());
            }
            
         
            addcol = addcol + 1;

            if (addcol == 1)
            {
                gvList.ClearSorting();
                gvList.ClearColumnsFilter();

                int insertRow = 0;
                DataTable dt = (gvList.GridControl.DataSource as DataTable);
                DataRow dtRow = dt.NewRow();
                dt.Rows.InsertAt(dtRow, insertRow);
                dt.AcceptChanges();

                //숨겨진 BIZ_YN 컬럼의 값 중 아무 열의 값을 가져오는 부분
                string start_date = gvList.GetRowCellValue(1, "BIZ_YN").ToNullSafeString();

                gvList.Focus();
                gvList.FocusedRowHandle = insertRow;
                gvList.FocusedColumn = gvList.Columns[0];

                gvList.SetRowCellValue(0, "CHK", 1);


                          
                gvList.SetRowCellValue(0, "BASE_DATE", start_date);
                gvList.SetRowCellValue(0, "END_DATE", "99991231");
                //이 부분을 기존 데이터 사라지지 않게 할 때 사용해야 함
                innertext = CEumCommon.GetObjToStr(cboType.EditValue.ToNullSafeString());
                for (int row = 0; row < gvList.RowCount; row++)
                {
                    gvList.SetRowCellValue(row, "INDEX_DIV", innertext);

                    
                    if (gvList.Columns[4].FieldName == "UNDER_DATA" && gvList.GetRowCellValue(row, "CHK").ToNullSafeString() == "1")
                    {
                        //금리 column 활성화
                        gvList.Columns[4].OptionsColumn.AllowEdit = true;

                    }


                }
               

                //그리드에 콤보박스 세팅하기

                RepositoryItemLookUpEdit repositoryItemLookUpEdit = new RepositoryItemLookUpEdit();

                repositoryItemLookUpEdit.NullText = "";

                gvList.Columns[1].AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
                gvList.Columns[1].ColumnEdit = (RepositoryItem)repositoryItemLookUpEdit;

                ComboUtil.SetComboGrid(oDataTransfer, gvList.Columns["INDEX_DIV"], "EC909", "CODE", "", false);
                //gvList.Columns[1].AppearanceCell.BackColor = Color.LightYellow;
                gvList.Columns[4].AppearanceCell.BackColor = Color.LightYellow;


            }

            else
            {
                CEumCommon.ShowEumMessageBox("최대 입력할 수 있는 행을 초과합니다.");
            
            }



        }

        private void dtBaseDate_EditValueChanged_1(object sender, EventArgs e)
        {

        }

        private void dtLastDate_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void labelControl8_Click(object sender, EventArgs e)
        {

        }

        private void dateFromDate_EditValueChanged(object sender, EventArgs e)
        {
            dateToDate.EditValue = DateTime.Today;
            
            if (dateToDate.DateTime != null)
                dateToDate.DateTime = dateToDate.DateTime.AddDays(0);
        }

        private void gcList_Click(object sender, EventArgs e)
        {

        }

        //추가한 행만 활성화 되도록 하는 부분       
        private void gvList_ShowingEditor(object sender, CancelEventArgs e)
        {            
              if (gvList.FocusedRowHandle >= 0)
                {
                    if (gvList.FocusedColumn.FieldName == "UNDER_DATA")
                    {
                        string temp = gvList.GetRowCellValue(gvList.FocusedRowHandle, "CHK").ToNullSafeString();
                        if (temp == "0")
                        {
                            e.Cancel = true;
                            
                        }
                        
                    }
                }             
        }

        
        private void gvList_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            //오늘날짜 빨간색으로 바꾸기 위한 변수
            string s5 = DateTime.Now.ToShortDateString();

            if (e.RowHandle < 0)
                return;

            //오늘 날짜인 경우 주황색으로 표시
            for (int row = 0; row < gvList.RowCount; row++)
            {
                if (e.Column.AbsoluteIndex == gvList.Columns["INPUT_DTIME"].AbsoluteIndex )
                {
                    if (gvList.GetRowCellValue(e.RowHandle, gvList.Columns[6]).ToNullSafeString().Length > 11)
                    {
                        if (gvList.GetRowCellValue(e.RowHandle, gvList.Columns[6]).ToNullSafeString().Substring(0, 10) == s5)
                        {
                            e.Appearance.BackColor = Color.AntiqueWhite;  
                        }     
                    }
                    
                }

            }

        }

   
       

       

       
       

      
    }
}
