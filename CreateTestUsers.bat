mru -u admin01 -p 123456 -e admin01@bespoke.com.my -r admin_user -c .\source\web\web.sph\Web.config
mru -u supervisor01 -p 123456 -e supervisor01@bespoke.com.my -r can_assign_maintenance_officer -c .\source\web\web.sph\Web.config
mru -u officer01 -p 123456 -e supervisor01@bespoke.com.my -r maintenance_officer -r can_inspect -c .\source\web\web.sph\Web.config