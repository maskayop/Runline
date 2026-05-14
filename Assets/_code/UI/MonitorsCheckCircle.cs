namespace Runline
{
    public class MonitorsCheckCircle : CheckCircle
    {
        MonitorsPanel monitorsPanel;

        protected override void OnInit()
        {
            monitorsPanel = MonitorsPanel.Instance;
        }

        public override void OnClick()
        {
            monitorsPanel.SelectMonitor(id);
        }
    }
}
