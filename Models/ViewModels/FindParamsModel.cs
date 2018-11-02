using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OldOneWinDB.Models
{
    public class FindParamsModel
    {
        [Display(Name = "Номер дела")]
        public int DocNo { get; set; }
        [Display(Name = "Имя")]
        public string Fname { get; set; }
        [Display(Name = "Отчество")]
        public string Mname { get; set; }
        [Display(Name = "Фамилия")]
        public string Lname { get; set; }
        [Display(Name = "Адрес")]
        public string Adres { get; set; }
        [Display(Name = "Обращение с")]
        public DateTime StartGettingDate { get; set; }
        [Display(Name = "Передали в отдел с")]
        public DateTime StartOutDeptDate { get; set; }
        [Display(Name = "Вернули из отдела с")]
        public DateTime StartReturnInDeptDate { get; set; }
        [Display(Name = "Выдали с")]
        public DateTime StartIssueDate { get; set; }
        [Display(Name = "Срок исполнения с")]
        public DateTime StartMustBeReadyDate { get; set; }
        [Display(Name = "Обращение до")]
        public DateTime EndGettingDate { get; set; }
        [Display(Name = "Передали в отдел по")]
        public DateTime EndOutDeptDate { get; set; }
        [Display(Name = "Вернули из отдела по")]
        public DateTime EndReturnInDeptDate { get; set; }
        [Display(Name = "Выдали по")]
        public DateTime EndIssueDate { get; set; }
        [Display(Name = "Срок исполнения по")]
        public DateTime EndMustBeReadyDate { get; set; }
        [Display(Name = "Дата решения")]

        public DateTime? DateSsolutions { get; set; }
        [Display(Name = "Номер решения")]
        public string NumberSolutions { get; set; }
        [Display(Name ="Тип решения")]
        public bool? ResultType { get; set; }
        [Display(Name = "Раздел")]
        public Guid SectionId { get; set; }
        [Display(Name = "Процедура")]
        public Guid ProcedureId { get; set; }

        public FindParamsModel()
        {
            ProcedureId = Guid.Empty;
        }
    }
}
