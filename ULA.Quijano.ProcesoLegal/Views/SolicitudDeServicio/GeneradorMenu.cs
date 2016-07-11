using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ULA.Quijano.Model.ModeloDB;
//using ULA.Quijano.ProcesoLegal.FormComplement;
using Ultimus.UtilityLayer;

namespace ULA.Quijano.ProcesoLegal
{
    public class GeneradorMenu
    {
        LogFilecs log = new LogFilecs();
        string ApplicationName = "Generador Menú";
        public ICollection<StepFormsList> GetFormsByProcessAndStep(string Process, string Step)
        {
            ICollection<StepFormsList> ret = new List<StepFormsList>();
            try
            {
                using (ModeloQuijano dbContext = new ModeloQuijano())
                {
                    ICollection<BPM_FORMSPROCESS> FormsProcesslist = dbContext.BPM_FORMSPROCESS.Where(
                                                q => q.BPM_CATSTEPS.STEPNAME == Step && q.BPM_CATSTEPS.BPM_CATPROCESSES.PROCESSNAME == Process)
                                                .OrderBy(d => d.FORMORDER).ToList();

                    foreach (var item in FormsProcesslist)
                    {
                        StepFormsList newItem = new StepFormsList();
                        newItem.FormFile = item.BPM_CATFORMS.FORMFILE;
                        newItem.FormName = item.BPM_CATFORMS.FORMLABEL;
                        newItem.FormPath = item.BPM_CATFORMS.BPM_CATWEBAPLICATIONS.WEBAPLICATIONPATH
                                           + item.BPM_CATFORMS.FORMFILE;
                        newItem.FormOrder = item.FORMORDER;
                        ret.Add(newItem);
                    }
                }
            }
            catch (Exception ex)
            {
                log.EscribirArchivo(ApplicationName, ex);
            }
            return ret;
        }
    }
}